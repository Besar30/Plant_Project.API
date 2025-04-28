
namespace Plant_Project.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PlantDetectionController : ControllerBase
	{
		private readonly IPlantDetectionService _plantDetectionService;

		public PlantDetectionController(IPlantDetectionService plantDetectionService)
		{
			_plantDetectionService = plantDetectionService ?? throw new ArgumentNullException(nameof(plantDetectionService));
		}

		[HttpPost("detect")]
		public async Task<IActionResult> DetectPlant([FromForm] IFormFile file, CancellationToken cancellationToken)
		{
			if (file == null || file.Length == 0)
				return BadRequest("No file uploaded.");

			try
			{
				var result = await _plantDetectionService.DetectPlantAsync(file, cancellationToken);
				return Ok(result);
			}
			catch (Exception ex)
			{
				// Handle error and provide user-friendly error message
				return StatusCode(500, new { message = "Error processing uploaded file", details = ex.Message });
			}
		}
	}
}
