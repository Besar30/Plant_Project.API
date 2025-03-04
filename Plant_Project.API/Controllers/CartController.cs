namespace Plant_Project.API.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class CartController(ICartServices cartServices) : ControllerBase
{
	private readonly ICartServices _cartServices = cartServices;

	[HttpGet("{userId}")]
	public async Task<IActionResult> GetAll([FromRoute] string userId, CancellationToken cancellationToken)
	{
		var result = await _cartServices.GetAllAsync(userId, cancellationToken);

		if (!result.IsSuccess || result.Value is null || result.Value.Count == 0)
			return result.ToProblem();

		return Ok(result.Value); 
	}
	[HttpPost("")]
	public async Task<IActionResult> AddToCart([FromBody] CartRequest request, CancellationToken cancellationToken)
	{
		var result = await _cartServices.AddToCartAsync(request, cancellationToken);

		return result.IsSuccess ? Ok() : result.ToProblem();
	}

	[HttpPut("")]
	public async Task<IActionResult> UpdateCartItem([FromBody] UpdateRequest request, CancellationToken cancellationToken)
	{
		var result = await _cartServices.UpdateQuantityAsync(request, cancellationToken);

		return result.IsSuccess ? NoContent() : result.ToProblem();
	}

	[HttpDelete("{cartId}")]
	public async Task<IActionResult> RemoveCartItem([FromRoute] int cartId, CancellationToken cancellationToken)
	{
		var result = await _cartServices.DeleteAsync(cartId, cancellationToken);

		return result.IsSuccess ? NoContent() : result.ToProblem();
	}

	[HttpDelete("clear/{userId}")]
	public async Task<IActionResult> ClearCart([FromRoute] string userId, CancellationToken cancellationToken)
	{
		var result = await _cartServices.ClearCartAsync(userId, cancellationToken);

		return result.IsSuccess ? NoContent() : result.ToProblem();
	}


}
