using Microsoft.AspNetCore.Mvc;
using Plant_Project.API.contracts.Cart;

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
			return result.ToProblem(StatusCodes.Status404NotFound);

		return Ok(result.Value); 
	}
	[HttpPost("")]
	public async Task<IActionResult> AddToCart([FromBody] CartRequest request, CancellationToken cancellationToken)
	{
		var result = await _cartServices.AddToCartAsync(request, cancellationToken);

		return result.IsSuccess ? Ok() : result.ToProblem(StatusCodes.Status400BadRequest);
	}

	[HttpPut("")]
	public async Task<IActionResult> UpdateCartItem([FromBody] UpdateRequest request, CancellationToken cancellationToken)
	{
		var result = await _cartServices.UpdateQuantityAsync(request, cancellationToken);

		return result.IsSuccess ? NoContent() : result.ToProblem(StatusCodes.Status404NotFound);
	}

	[HttpDelete("{cartId}")]
	public async Task<IActionResult> RemoveCartItem([FromRoute] int cartId, CancellationToken cancellationToken)
	{
		var result = await _cartServices.DeleteAsync(cartId, cancellationToken);

		return result.IsSuccess ? NoContent() : result.ToProblem(StatusCodes.Status404NotFound);
	}

	[HttpDelete("clear/{userId}")]
	public async Task<IActionResult> ClearCart([FromRoute] string userId, CancellationToken cancellationToken)
	{
		var result = await _cartServices.ClearCartAsync(userId, cancellationToken);

		return result.IsSuccess ? NoContent() : result.ToProblem(StatusCodes.Status400BadRequest);
	}
}