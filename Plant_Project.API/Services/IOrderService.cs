using Plant_Project.API.contracts.Order;

namespace Plant_Project.API.Services;

public interface IOrderService
{
	Task<Result<List<OrderResponse>>> ShowOrderAsync(OrderRequest request, CancellationToken cancellationToken);
	Task<List<OrderResponse>> GetAllOrdersByUserAsync(string userId, CancellationToken cancellationToken);
	Task<Result> DeleteOrderAsync(int orderId, CancellationToken cancellationToken = default);
	Task<Result> UpdateOrderAsync(UpdateOrderRequest request, CancellationToken cancellationToken = default);

}
