using Plant_Project.API.contracts.Payment;


namespace Plant_Project.API.Services;

public class PaymentService : IPaymentService
{
	private readonly ApplicationDbContext _context;
	private readonly PayAuthService _payAuthService;
	private readonly UserManager<ApplicationUser> _userManager;

	public PaymentService(UserManager<ApplicationUser> userManager, ApplicationDbContext context, PayAuthService payAuthService)
	{
		_userManager = userManager;
		_context = context;
		_payAuthService = payAuthService;
	}

	public async Task<Result> CheckoutAsync(CheckoutRequest request, CancellationToken cancellationToken)
	{
		try
		{
			var cartItems = await _context.Carts
				.Where(c => c.UserId == request.UserId)
				.Include(c => c.Plant)
				.ToListAsync(cancellationToken);

			if (cartItems.Count == 0)
				return Result.Failure(CartErrors.CartEmpty);

			// Prepare Order object (not saved yet)
			var order = new Order
			{
				UserId = request.UserId,
				TotalAmount = cartItems.Sum(c => c.TotalPrice),
				OrderDate = DateTime.UtcNow,
				PaymentMethod = request.PaymentMethod,
				Address = request.Address,
				OrderItems = cartItems.Select(c => new OrderItem
				{
					PlantId = c.PlantId,
					PlantName = c.PlantName,
					Quantity = c.Quantity,
					Price = c.Price,
					UserId = request.UserId
				}).ToList()
			};

			// 🟢 Cash Payment (save directly as Pending)
			if (request.PaymentMethod.Equals("Cash", StringComparison.OrdinalIgnoreCase))
			{
				order.PaymentStatus = "Pending";
				_context.Orders.Add(order);
				await _context.SaveChangesAsync(cancellationToken);

				_context.Carts.RemoveRange(cartItems);
				await _context.SaveChangesAsync(cancellationToken);

				return Result.Success();
			}

			// 💳 Card Payment (Visa / CreditCard)
			if (request.CardDetails == null)
				return Result.Failure(PaymentErrors.MissingCardDetails);

			// Normalize cardType
			string cardType = request.CardDetails.CardType?.Trim().ToLowerInvariant();

			if (!CheckoutValidator.IsValidCardNumber(request.CardDetails.CardNumber))
				return Result.Failure(PaymentErrors.InValidCardNumber);

			if (!CheckoutValidator.IsValidExpiryDate(request.CardDetails.ExpirationDate))
				return Result.Failure(PaymentErrors.InValidExpiryDate);

			if (!CheckoutValidator.IsValidCVV(request.CardDetails.CVV, cardType))
				return Result.Failure(PaymentErrors.InValidCVV);

			// Process payment
			var paymentResult = await ProcessPaymentAsync(order, request.PaymentMethod, request.CardDetails);

			if (!paymentResult.Success)
				return Result.Failure(PaymentErrors.PaymentFailer);

			// ✅ Save only if payment succeeded
			order.PaymentStatus = "Paid";
			order.TransactionId = paymentResult.TransactionId;
			_context.Orders.Add(order);
			await _context.SaveChangesAsync(cancellationToken);

			// Add payment record
			var payment = new Payment
			{
				UserId = order.UserId,
				OrderId = order.Id,
				Amount = order.TotalAmount,
				Currency = "USD",
				PaymentIntentId = order.TransactionId ?? string.Empty,
				Status = "Succeeded",
				CreatedAt = DateTime.UtcNow
			};

			_context.Payments.Add(payment);
			_context.Carts.RemoveRange(cartItems);
			await _context.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Checkout Error: {ex.Message}\n{ex.StackTrace}");
			return Result.Failure(PaymentErrors.InternalError);
		}
	}

	private async Task<PaymentResult> ProcessPaymentAsync(Order order, string paymentMethod, PaymentCardDetails cardDetails)
	{
		bool isVisa = cardDetails.CardNumber.StartsWith("4");

		if (paymentMethod.Equals("CreditCard", StringComparison.OrdinalIgnoreCase) ||
			paymentMethod.Equals("Visa", StringComparison.OrdinalIgnoreCase))
		{
			if (paymentMethod.Equals("Visa", StringComparison.OrdinalIgnoreCase) && !isVisa)
				return new PaymentResult(false, "Invalid Visa card");

			var result = await _payAuthService.ProcessPaymentAsync(
				order.TotalAmount,
				cardDetails.CardNumber,
				cardDetails.ExpirationDate,
				cardDetails.CVV
			);

			return result;
		}

		return new PaymentResult(false, "Invalid payment method");
	}
}
