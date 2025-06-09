using Plant_Project.API.contracts.Payment;

namespace Plant_Project.API.Controllers;

//[Authorize] 
[Route("payment")]
[ApiController]
public class PaymentController(IPaymentService paymentService) : ControllerBase
{
	private readonly IPaymentService _paymentService = paymentService;

	[HttpPost("checkout")]
	public async Task<IActionResult> Checkout([FromBody] CheckoutRequest request, CancellationToken cancellationToken)
	{
		try
		{
			if (request.PaymentMethod != "Cash" && request.CardDetails == null)
			{
				return BadRequest("Credit card details are required for non-cash payments.");
			}


			var result = await _paymentService.CheckoutAsync(request, cancellationToken);

			return result.IsSuccess ? NoContent() : result.ToProblem(StatusCodes.Status400BadRequest);
		}
		catch (Exception ex)
		{
			return StatusCode(404, new { error = ex.Message, stack = ex.StackTrace });
		}
	}

}

