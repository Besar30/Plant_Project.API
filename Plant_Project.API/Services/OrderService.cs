using Plant_Project.API.contracts.Order;

namespace Plant_Project.API.Services;

public class OrderService : IOrderService
{
	private readonly ApplicationDbContext _context;
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly ILogger<OrderService> _logger;
	public OrderService(UserManager<ApplicationUser> userManager, ApplicationDbContext context, ILogger<OrderService> logger)
	{
		_userManager = userManager;
		_context = context;
		_logger = logger;
	}

	public async Task<Result<List<OrderResponse>>> ShowOrderAsync(OrderRequest request, CancellationToken cancellationToken)
	{
		var user = await _userManager.FindByIdAsync(request.UserId);
		if (user == null)
			return Result.Failure<List<OrderResponse>>(UeserError.UserNotFound);

		if (!request.OrdrerId.HasValue)
			return Result.Failure<List<OrderResponse>>(OrderErrors.OrderNotFound);

		var order = await _context.Orders
			.Include(o => o.OrderItems)
			.FirstOrDefaultAsync(o => o.Id == request.OrdrerId && o.UserId == request.UserId, cancellationToken);

		if (order == null)
			return Result.Failure<List<OrderResponse>>(OrderErrors.OrderNotFound);

		if (order.OrderItems == null || !order.OrderItems.Any())
			return Result.Failure<List<OrderResponse>>(OrderErrors.ItemNotFound);

		var responseList = order.OrderItems.Select(item => new OrderResponse(
			UserId: order.UserId,
			OrderId: order.Id,
			PlantName: item.PlantName,
			Address: order.Address,
			PaymentMethod: order.PaymentMethod,
			TotalAmount: order.TotalAmount,
			OrderDate: order.OrderDate,
			PaymentStatus: order.PaymentStatus
		)).ToList();

		return Result.Success(responseList);
	}

	public async Task<List<OrderResponse>> GetAllOrdersByUserAsync(string userId, CancellationToken cancellationToken)
	{
		var orders = await _context.Orders
			.Where(o => o.UserId == userId)
			.Include(o => o.OrderItems)
			.ToListAsync(cancellationToken);

		var response = new List<OrderResponse>();

		foreach (var order in orders)
		{
			foreach (var item in order.OrderItems)
			{
				response.Add(new OrderResponse(
					UserId: order.UserId,
					OrderId: order.Id,
					PlantName: item.PlantName,
					Address: order.Address,
					PaymentMethod: order.PaymentMethod,
					TotalAmount: order.TotalAmount,
					OrderDate: order.OrderDate,
					PaymentStatus: order.PaymentStatus
				));
			}
		}

		return response;
	}




	public async Task<Result> DeleteOrderAsync(int orderId, CancellationToken cancellationToken = default)
	{
		var order = await _context.Orders
			.Include(o => o.Payments)
			.FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

		if (order == null)
			return Result.Failure(new Error("ORDER_NOT_FOUND", "Order not found."));

		// Delete related payments first
		var payments = await _context.Payments
			.Where(p => p.OrderId == orderId)
			.ToListAsync(cancellationToken);

		_context.Payments.RemoveRange(payments);
		_context.Orders.Remove(order);

		await _context.SaveChangesAsync(cancellationToken);
		return Result.Success();
	}


	public async Task<Result> UpdateOrderAsync(UpdateOrderRequest request, CancellationToken cancellationToken = default)
	{
			var order = await _context.Orders.FindAsync(request.OrderId);

			if (order == null)
				return Result.Failure(new Error("ORDER_NOT_FOUND", "Order not found."));

			if (order.PaymentStatus == "Paid")
			{
				return Result.Failure(new Error("ORDER_ALREADY_PAID", "Cannot update a paid order."));
			}

			order.Address = request.Address;
			order.PaymentMethod = request.PaymentMethod;

			await _context.SaveChangesAsync(cancellationToken);

			return Result.Success();
	}
}

