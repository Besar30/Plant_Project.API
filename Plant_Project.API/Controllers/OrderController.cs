namespace Plant_Project.API.Controllers;

using global::Plant_Project.API.contracts.Order;
using Microsoft.AspNetCore.Mvc;


//[Authorize]
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

	[HttpGet("user/{userId}")]
	public async Task<IActionResult> GetAllOrdersForUser(string userId, CancellationToken cancellationToken)
	{
		var orders = await _orderService.GetAllOrdersByUserAsync(userId, cancellationToken);
		return Ok(orders);
	}

	[HttpDelete("{orderId}")]
	public async Task<IActionResult> DeleteOrder(int orderId, CancellationToken cancellationToken)
	{
		var result = await _orderService.DeleteOrderAsync(orderId, cancellationToken);

		if (result.IsFailure)
			return BadRequest($"Fail to Delete{orderId}");

		return Ok("Order deleted successfully.");
	}

	[HttpPut]
	public async Task<IActionResult> UpdateOrder(UpdateOrderRequest request, CancellationToken cancellationToken)
	{
		var result = await _orderService.UpdateOrderAsync(request, cancellationToken);

		if (result.IsFailure)
			return BadRequest($"Fail to Update Order");

		return Ok("Order updated successfully.");
	}
}



