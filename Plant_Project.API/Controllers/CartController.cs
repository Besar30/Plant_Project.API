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

        if (!result.IsSuccess)
            return result.ToProblem(400);

        if (result.Value is null || result.Value.Count == 0)
            return NotFound("Cart is empty.");

        return Ok(result.Value);
    }

    [HttpPost("")]
    public async Task<IActionResult> AddToCart([FromBody] CartRequest request, CancellationToken cancellationToken)
    {
        var result = await _cartServices.AddToCartAsync(request, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem(400);
    }

    [HttpPut("")]
    public async Task<IActionResult> UpdateCartItem([FromBody] UpdateRequest request, CancellationToken cancellationToken)
    {
        var result = await _cartServices.UpdateAsync(request, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem(404);
    }


    [HttpDelete("{userId}/{ItemId}")]
    public async Task<IActionResult> RemoveCartItem([FromRoute] string userId, [FromRoute] int ItemId, CancellationToken cancellationToken)
    {
        var result = await _cartServices.DeleteAsync(userId, ItemId, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem(400);
    }


    [HttpDelete("clear/{userId}")]
    public async Task<IActionResult> ClearCart([FromRoute] string userId, CancellationToken cancellationToken)
    {
        var result = await _cartServices.ClearCartAsync(userId, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem(404);
    }
}