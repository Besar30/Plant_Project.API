namespace Plant_Project.API.Services;

public class PaymentService(UserManager<ApplicationUser> userManager,
	ApplicationDbContext context,
	PayAuthService payAuthService
	) : IPaymentService
{
	private readonly ApplicationDbContext _context = context;
	private readonly PayAuthService _payAuthService = payAuthService;
	private readonly UserManager<ApplicationUser> _userManager = userManager;

	public async Task<Result> CheckoutAsync(CheckoutRequest request, PaymentCardDetails? cardDetails, CancellationToken cancellationToken)
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

		
		if (request.PaymentMethod != "Cash")
		{
			
			if (cardDetails == null)
				return Result.Failure(PaymentErrors.MissingCardDetails);

			var paymentResult = await ProcessPaymentAsync(order, request.PaymentMethod, cardDetails);

			if (!paymentResult.Success)
				return Result.Failure(PaymentErrors.PaymentFailer);

			order.PaymentStatus = "Paid";
			order.TransactionId = paymentResult.TransactionId;
			await _context.SaveChangesAsync(cancellationToken);
		}

		
		_context.Carts.RemoveRange(cartItems);
		await _context.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}

	private async Task<PaymentResult> ProcessPaymentAsync(Order order, string paymentMethod, PaymentCardDetails cardDetails)
	{
		if (paymentMethod == "CreditCard" || paymentMethod == "Visa")
		{
			
			bool isVisa = cardDetails.CardNumber.StartsWith("4");

			var paymentResult = await _payAuthService.ProcessPaymentAsync(
				order.TotalAmount,
				cardDetails.CardNumber,
				cardDetails.ExpirationDate,
				cardDetails.CVV
			);

			// If the method was explicitly Visa but the card isn't Visa, return an error
			if (paymentMethod == "Visa" && !isVisa)
				return new PaymentResult(false, "Invalid Visa card");

			return paymentResult;
		}

		return new PaymentResult(false, "Invalid payment method");
	}
}
