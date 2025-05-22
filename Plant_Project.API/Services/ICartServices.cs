using Plant_Project.API.contracts.Cart;

namespace Plant_Project.API.Services;

public interface ICartServices
{
	Task<Result<List<CartResponse>>> GetAllAsync(string userId, CancellationToken cancellationToken = default);
	Task<Result> AddToCartAsync(CartRequest request, CancellationToken cancellationToken = default);
	Task<Result> UpdateQuantityAsync(UpdateRequest request, CancellationToken cancellationToken);
	Task<Result> DeleteAsync(int cartId, CancellationToken cancellationToken);
	Task<Result> ClearCartAsync(string userId, CancellationToken cancellationToken);
	
}