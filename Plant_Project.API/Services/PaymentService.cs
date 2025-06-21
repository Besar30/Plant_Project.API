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

			// ✅ 1️⃣ Check stock availability BEFORE placing order
			foreach (var cartItem in cartItems)
			{
				if (cartItem.Plant.Quantity < cartItem.Quantity)
				{
					return Result.Failure(new Error("OutOfStock", $"Insufficient stock for {cartItem.Plant.Name}. Available: {cartItem.Plant.Quantity}"));
				}
			}

			// ✅ 2️⃣ Prepare Order object (not saved yet)
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

			// ✅ 3️⃣ Handle Cash Payment
			if (request.PaymentMethod.Equals("Cash", StringComparison.OrdinalIgnoreCase))
			{
				order.PaymentStatus = "Pending";
				_context.Orders.Add(order);
				await _context.SaveChangesAsync(cancellationToken);

				// Decrease stock after saving order
				foreach (var item in order.OrderItems)
				{
					var plant = await _context.plants.FindAsync(item.PlantId);
					if (plant != null)
					{
						plant.Quantity -= item.Quantity;
					}
				}
				await _context.SaveChangesAsync(cancellationToken);

				_context.Carts.RemoveRange(cartItems);
				await _context.SaveChangesAsync(cancellationToken);

				return Result.Success();
			}

			// ✅ 4️⃣ Handle Card Payment
			if (request.CardDetails == null)
				return Result.Failure(PaymentErrors.MissingCardDetails);

			string cardType = request.CardDetails.CardType?.Trim().ToLowerInvariant();

			if (!CheckoutValidator.IsValidCardNumber(request.CardDetails.CardNumber))
				return Result.Failure(PaymentErrors.InValidCardNumber);

			if (!CheckoutValidator.IsValidExpiryDate(request.CardDetails.ExpirationDate))
				return Result.Failure(PaymentErrors.InValidExpiryDate);

			if (!CheckoutValidator.IsValidCVV(request.CardDetails.CVV, cardType))
				return Result.Failure(PaymentErrors.InValidCVV);

			var paymentResult = await ProcessPaymentAsync(order, request.PaymentMethod, request.CardDetails);
			if (!paymentResult.Success)
				return Result.Failure(PaymentErrors.PaymentFailer);

			order.PaymentStatus = "Paid";
			order.TransactionId = paymentResult.TransactionId;
			_context.Orders.Add(order);
			await _context.SaveChangesAsync(cancellationToken);

			// ✅ 5️⃣ Decrease stock after payment successful
			foreach (var item in order.OrderItems)
			{
				var plant = await _context.plants.FindAsync(item.PlantId);
				if (plant != null)
				{
					plant.Quantity -= item.Quantity;
				}
			}
			await _context.SaveChangesAsync(cancellationToken);

			// ✅ 6️⃣ Save Payment record
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
			await _context.SaveChangesAsync(cancellationToken);

			// ✅ 7️⃣ Clear user's cart
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
