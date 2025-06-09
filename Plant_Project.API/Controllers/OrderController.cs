namespace Plant_Project.API.Controllers;

using global::Plant_Project.API.contracts.Order;
using Microsoft.AspNetCore.Mvc;



[ApiController]
[Route("order")]
public class OrderController(IOrderService orderService) : ControllerBase
{
	private readonly IOrderService _orderService = orderService;

	[HttpPost("details")]
	public async Task<IActionResult> GetOrder([FromBody] OrderRequest request, CancellationToken cancellationToken)
	{
		var result = await _orderService.ShowOrderAsync(request, cancellationToken);
		return result.IsSuccess
			? Ok(result.Value)
			: result.ToProblem(StatusCodes.Status404NotFound);
	}
}

