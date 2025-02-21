using Microsoft.AspNetCore.Mvc;
using Plant_Project.API.contracts.Plants;
using Plant_Project.API.Services;
namespace Plant_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantController(IplantServices iplantServices) : ControllerBase
    {
        private readonly IplantServices _iplantServices = iplantServices;

        [HttpGet("")]
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        {
            var result=await _iplantServices.GetAllAsync(cancellationToken);
            return Ok(result);
        }
        [HttpPost("")]
        public async Task<IActionResult> AddPlantAsync([FromForm] PlantsRequest request,CancellationToken cancellationToken)
        {
            var result= await _iplantServices.AddPlantAsync(request,cancellationToken);

            return result.IsSuccess ?
                Ok() : result.ToProblem(StatusCodes.Status400BadRequest);

        }
    }
}
