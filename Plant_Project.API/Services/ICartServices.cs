using Plant_Project.API.contracts.Cart;

namespace Plant_Project.API.Services;

public interface ICartServices
{
    Task<Result<List<CartResponse>>> GetAllAsync(string userId, CancellationToken cancellationToken = default);
    Task<Result> AddToCartAsync(CartRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(UpdateRequest request, CancellationToken cancellationToken);
    Task<Result> DeleteAsync(string userId, int plantId, CancellationToken cancellationToken);
    Task<Result> ClearCartAsync(string userId, CancellationToken cancellationToken);

}