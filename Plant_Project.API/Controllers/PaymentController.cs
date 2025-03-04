
namespace Plant_Project.API.Controllers;

	[Authorize] 
	[Route("payment")]
	[ApiController]
	public class PaymentController(IPaymentService paymentService) : ControllerBase
	{
		private readonly IPaymentService _paymentService = paymentService;

		[HttpPost("checkout")]
		public async Task<IActionResult> Checkout([FromBody] CheckoutRequest request, CancellationToken cancellationToken)
		{
			// Ensure payment method is provided
			if (string.IsNullOrEmpty(request.PaymentMethod))
				return Result.Failure(PaymentErrors.PaymentMethodRequaird);

			// Validate card details for non-cash payments
			if (request.PaymentMethod != "Cash" && request.CardDetails == null)
				return BadRequest(new { message = "Card details are required for card payments" });

			// Process checkout
			var result = await _paymentService.CheckoutAsync(request, request.CardDetails, cancellationToken);

			// Return response
			return result.IsSuccess ? Ok(new { message = "Payment successful!" }) : BadRequest(result.ToProblem());
		}
	}

