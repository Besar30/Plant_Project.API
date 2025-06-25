using Microsoft.AspNetCore.Mvc;
using Plant_Project.API.contracts.Ai;
using Plant_Project.API.Services;

namespace Plant_Project.API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class PlantDetectionController : ControllerBase
	{
		private readonly IPlantDetectionService _plantDetectionService;

		public PlantDetectionController(IPlantDetectionService plantDetectionService)
		{
			_plantDetectionService = plantDetectionService;
		}

		[HttpPost("detect")]
		public async Task<IActionResult> DetectPlant([FromForm] IFormFile file, CancellationToken cancellationToken)
		{
			if (file == null || file.Length == 0)
			{
				return BadRequest(new PlantDetectionResponseDto
				{
					Success = false,
					Message = "No file uploaded.",
					Data = null
				});
			}

			var result = await _plantDetectionService.DetectPlantAsync(file, cancellationToken);

			if (!result.IsSuccess)
			{
				// Optional: return 500 if it's an internal error
				var status = result.error.Code == "AIError" || result.error.Code == "Exception" ? 500 : 400;

				return StatusCode(status, new PlantDetectionResponseDto
				{
					Success = false,
					Message = result.error.Discription,
					Data = null
				});
			}

			return Ok(new PlantDetectionResponseDto
			{
				Success = true,
				Message = "Plant detected successfully.",
				Data = result.Value
			});
		}
	}
}
