using Plant_Project.API.contracts.Order;

namespace Plant_Project.API.Services;

public interface IOrderService
{
	Task<Result<List<OrderResponse>>> ShowOrderAsync(OrderRequest request, CancellationToken cancellationToken);
}
