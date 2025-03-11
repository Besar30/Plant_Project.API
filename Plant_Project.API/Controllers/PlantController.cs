using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plant_Project.API.Abstraction.Consts;
using Plant_Project.API.Authentication.Filters;
using Plant_Project.API.contracts.Plants;
using Plant_Project.API.Services;
namespace Plant_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class PlantController(IplantServices iplantServices) : ControllerBase
    {
        private readonly IplantServices _iplantServices = iplantServices;
        //https://localhost:7286/api/Plant
        [HttpGet("")]
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        {
            var result=await _iplantServices.GetAllAsync(cancellationToken);
            return Ok(result);
        }
        //https://localhost:7286/api/Plant
        [Authorize(Roles = DefaultRoles.Admin)]
        [HttpPost("")]
        public async Task<IActionResult> AddPlantAsync([FromForm] PlantsRequest request,CancellationToken cancellationToken)
        {
            var result= await _iplantServices.AddPlantAsync(request,cancellationToken);

            return result.IsSuccess ?
                Ok() : result.ToProblem(StatusCodes.Status400BadRequest);

        }
        //https://localhost:7286/api/Plant/1
        [Authorize(Roles = DefaultRoles.Admin)]

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdatePlantAsync([FromRoute] int Id, [FromForm]  PlantsRequest request,CancellationToken cancellationToken)
        {
            var resutl= await _iplantServices.UpdatePlantAsync(Id,request,cancellationToken);

            return resutl.IsSuccess ?
                Ok():
                resutl.ToProblem(StatusCodes.Status400BadRequest);
        }
        //https://localhost:7286/api/Plant/2
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int Id,CancellationToken cancellationToken)
        {
            var result= await _iplantServices.GetByIdAsync(Id,cancellationToken);
            return result.IsSuccess ?
                Ok(result.Value) :
                result.ToProblem(StatusCodes.Status404NotFound);
        }
        //https://localhost:7286/api/Plant/GetByName
        [HttpGet("GetByName")]
        public async Task<IActionResult> GetByNameAsync([FromBody] string Name,CancellationToken cancellationToken)
        {
            var result= await _iplantServices.GetByNameAsync(Name,cancellationToken);
            return result.IsSuccess ?
                Ok(result.Value) :
                result.ToProblem(StatusCodes.Status404NotFound);
        }
        [Authorize(Roles = DefaultRoles.Admin)]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int Id,CancellationToken cancellationToken)
        {
            var result= await _iplantServices.DeleteAsync(Id,cancellationToken);
            return result.IsSuccess ?
                Ok() :
                result.ToProblem(StatusCodes.Status404NotFound);
        }

    }
}
