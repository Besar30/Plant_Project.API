using Plant_Project.API.contracts.Payment;

namespace Plant_Project.API.Services;

public class PaymentService(UserManager<ApplicationUser> userManager,
	ApplicationDbContext context,
	PayAuthService payAuthService
	) : IPaymentService
{
	private readonly ApplicationDbContext _context = context;
	private readonly PayAuthService _payAuthService = payAuthService;
	private readonly UserManager<ApplicationUser> _userManager = userManager;

	public async Task<Result> CheckoutAsync(CheckoutRequest request, CancellationToken cancellationToken)
	{
		var cartItems = await _context.Carts
			.Where(c => c.UserId == request.UserId)
			.Include(c => c.Plant)
			.ToListAsync(cancellationToken);

		if (cartItems.Count == 0)
			return Result.Failure(CartErrors.CartEmpty);

		var order = new Order
		{
			UserId = request.UserId,
			TotalAmount = cartItems.Sum(c => c.TotalPrice),
			OrderDate = DateTime.UtcNow,
			PaymentMethod = request.PaymentMethod,
			PaymentStatus = request.PaymentMethod == "Cash" ? "Pending" : "Processing",
			Address = request.Address, // Save user-provided address
			OrderItems = cartItems.Select(c => new OrderItem
			{
				PlantId = c.PlantId,
				PlantName = c.PlantName,
				Quantity = c.Quantity,
				Price = c.Price
			}).ToList()
		};

		_context.Orders.Add(order);
		await _context.SaveChangesAsync(cancellationToken);

		if (request.PaymentMethod == "Cash")
		{
			_context.Carts.RemoveRange(cartItems);
			await _context.SaveChangesAsync(cancellationToken);
			return Result.Success();
		}

		if (request.PaymentMethod != "Cash")
		{
			// Ensure card details are provided
			if (request.CardDetails == null)
				return Result.Failure(PaymentErrors.MissingCardDetails);

			// Validate Card Details
			if (!CheckoutValidator.IsValidCardNumber(request.CardDetails.CardNumber))
				return Result.Failure(PaymentErrors.InValidCardNumber);

			if (!CheckoutValidator.IsValidExpiryDate(request.CardDetails.ExpirationDate))
				return Result.Failure(PaymentErrors.InValidExpiryDate);

			if (!CheckoutValidator.IsValidCVV(request.CardDetails.CVV, request.CardDetails.CardType))
				return Result.Failure(PaymentErrors.InValidCVV);

			// Process Payment
			var paymentResult = await ProcessPaymentAsync(order, request.PaymentMethod, request.CardDetails!);

			if (!paymentResult.Success)
				return Result.Failure(PaymentErrors.PaymentFailer);
			order.TransactionId = paymentResult.TransactionId;
			order.PaymentStatus = "Paid";
			await _context.SaveChangesAsync(cancellationToken);


			var payment = new Payment
			{
				UserId = request.UserId,
				OrderId = order.Id,
				Amount = order.TotalAmount,
				Currency = "USD", 
				PaymentIntentId = order.TransactionId ?? string.Empty,
				Status = "Succeeded",
				CreatedAt = DateTime.UtcNow
			};

			_context.Payments.Add(payment);

			await _context.SaveChangesAsync(cancellationToken);
		}
		_context.Carts.RemoveRange(cartItems);

		await _context.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}


	private async Task<PaymentResult> ProcessPaymentAsync(Order order, string paymentMethod, PaymentCardDetails cardDetails)
	{
		bool isVisa = cardDetails.CardNumber.StartsWith("4");

		if (paymentMethod == "CreditCard" || paymentMethod == "Visa")
		{
			var paymentResult = await _payAuthService.ProcessPaymentAsync(
				order.TotalAmount,
				cardDetails.CardNumber,
				cardDetails.ExpirationDate,
				cardDetails.CVV
			);

			if (paymentMethod == "Visa" && !isVisa)
				return new PaymentResult(false, "Invalid Visa card");

			return paymentResult;
		}

		return new PaymentResult(false, "Invalid payment method");
	}

}
