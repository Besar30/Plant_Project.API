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
				return BadRequest("No file uploaded.");

			var result = await _plantDetectionService.DetectPlantAsync(file, cancellationToken);

			if (!result.IsSuccess)
			{
				return BadRequest(new PlantDetectionResponseDto
				{
					Success = false,
					Message = result.error.Discription
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
