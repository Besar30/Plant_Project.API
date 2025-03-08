using Microsoft.AspNetCore.Mvc;
using Plant_Project.API.Authentication.Filters;
using Plant_Project.API.contracts.Roles;

namespace Plant_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController (IRoleServices roleServices): ControllerBase
    {
        private readonly IRoleServices _roleServices = roleServices;
        [HttpGet("")]
        [HasPermission(Permissions.GetRoles)]
        public async Task<IActionResult> GetAll([FromQuery] bool includeDisable, CancellationToken cancellationToken)
        {
            var result= await _roleServices.GetAllAsync(includeDisable,cancellationToken);
            return Ok(result);
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(string Id)
        {
            var result = await _roleServices.GetAsync(Id);
            return result.IsSuccess ?
                 Ok(result) : result.ToProblem(StatusCodes.Status404NotFound); 
        }
        [HttpPost("")]
        public async Task<IActionResult> AddAll([FromBody] RoleRequest request)
        {
            var result = await _roleServices.AddAsync(request);
            return result.IsSuccess ?
                 Ok(result) : result.ToProblem(StatusCodes.Status400BadRequest);

        }
    }
}