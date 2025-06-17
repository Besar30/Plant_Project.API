using Plant_Project.API.contracts.Order;

namespace Plant_Project.API.Services;

public class OrderService : IOrderService
{
	private readonly ApplicationDbContext _context;
	private readonly UserManager<ApplicationUser> _userManager;

	public OrderService(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
	{
		_userManager = userManager;
		_context = context;
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

}
