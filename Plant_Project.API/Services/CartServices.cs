using Plant_Project.API.contracts.Cart;

namespace Plant_Project.API.Services;

public class CartServices(UserManager<ApplicationUser> userManager,
	ApplicationDbContext context) : ICartServices
{
	private readonly ApplicationDbContext _context = context;
	private readonly UserManager<ApplicationUser> _userManager = userManager;
	public async Task<Result<List<CartResponse>>> GetAllAsync(string userId, CancellationToken cancellationToken = default)
	{
		var cartItems = await _context.Carts
			.Where(c => c.UserId == userId)
			.Include(c => c.Plant)
			.Select(c => new CartResponse(

				c.Id,
				c.PlantId,
				c.Plant.Name ?? string.Empty,
				c.Quantity,
				c.Price,
				c.Quantity * c.Price,
				c.ImagePath ?? string.Empty

			))
			.ToListAsync(cancellationToken);

		return Result.Success(cartItems);
	}

	public async Task<Result> AddToCartAsync(CartRequest request, CancellationToken cancellationToken = default)
	{
		var userExists = await _userManager.Users.AnyAsync(u => u.Id == request.UserId, cancellationToken);
		if (!userExists)
			return Result.Failure(UeserError.UserNotFound);

		var plant = await _context.plants.FindAsync(request.PlantId, cancellationToken);
		if (plant == null)
			return Result.Failure(PlantErrors.PlantNotFound);

		if (!plant.Is_Avilable || plant.Quantity < request.Quantity)
			return Result.Failure(PlantErrors.QuantityShortage);

		var cartItem = await _context.Carts
			.FirstOrDefaultAsync(c => c.UserId == request.UserId && c.PlantId == request.PlantId, cancellationToken);

		if (cartItem != null)
		{
			cartItem.Quantity += request.Quantity;
		}
		else
		{

			cartItem = new Cart
			{
				UserId = request.UserId,
				PlantId = request.PlantId,
				PlantName = plant.Name,
				Quantity = request.Quantity,
				Price = plant.Price,
				ImagePath = plant.ImagePath
			};

			await _context.Carts.AddAsync(cartItem, cancellationToken);
		}

		await _context.SaveChangesAsync(cancellationToken);
		return Result.Success();
	}

	public async Task<Result> UpdateAsync(UpdateRequest request, CancellationToken cancellationToken)
	{
		var cartItem = await _context.Carts
			.FirstOrDefaultAsync(x => x.UserId == request.UserId && x.PlantId == request.ItemId, cancellationToken);

		if (cartItem == null)
			return Result.Failure(CartErrors.ItemNotFound);

		cartItem.Quantity = request.Quantity;
		await _context.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}


	public async Task<Result> DeleteAsync(string userId, int plantId, CancellationToken cancellationToken)
	{
		var cartItem = await _context.Carts
			.FirstOrDefaultAsync(c => c.UserId == userId && c.PlantId == plantId, cancellationToken);

		if (cartItem == null)
			return Result.Failure(CartErrors.ItemNotFound);

		_context.Carts.Remove(cartItem);
		await _context.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}


	public async Task<Result> ClearCartAsync(string userId, CancellationToken cancellationToken)
	{
		var cartItems = await _context.Carts.Where(c => c.UserId == userId).ToListAsync(cancellationToken);
		if (cartItems.Count == 0) return Result.Failure(CartErrors.CartEmpty);

		_context.Carts.RemoveRange(cartItems);
		await _context.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}


}
