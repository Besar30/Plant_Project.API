using Microsoft.AspNetCore.Mvc;
using Plant_Project.API.Authentication.Filters;
using Plant_Project.API.contracts.Roles;
using Plant_Project.API.Migrations;

namespace Plant_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class RolesController (IRoleServices roleServices): ControllerBase
    {
        private readonly IRoleServices _roleServices = roleServices;
        //[HasPermission(Permissions.GetRoles)]   
        [Authorize(Roles =DefaultRoles.Admin)]
        [HttpGet("")]
        public async Task<IActionResult> GetAll([FromQuery] bool includeDisable, CancellationToken cancellationToken)
        {
            var result= await _roleServices.GetAllAsync(includeDisable,cancellationToken);
      
            return Ok(result);
        }
        [Authorize(Roles = DefaultRoles.Admin)]
        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(string Id)
        {
            var result = await _roleServices.GetAsync(Id);
            return result.IsSuccess ?
                 Ok(result) : result.ToProblem(StatusCodes.Status404NotFound); 
        }
        [Authorize(Roles = DefaultRoles.Admin)]
        [HttpPost("")]
        public async Task<IActionResult> AddAll([FromBody] RoleRequest request)
        {
            var result = await _roleServices.AddAsync(request);
            return result.IsSuccess ?
                 CreatedAtAction(nameof(Get),new {result.Value.Id,result.Value}) : result.ToProblem(StatusCodes.Status400BadRequest);

        }
        [Authorize(Roles = DefaultRoles.Admin)]
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateAll([FromRoute] string Id,[FromBody] RoleRequest request)
        {
            var result = await _roleServices.UpdateAsync(Id,request);
            return result.IsSuccess ?
                NoContent() : result.ToProblem(StatusCodes.Status400BadRequest);

        }
        [Authorize(Roles = DefaultRoles.Admin)]
        [HttpPut("{id}/toggle-status")]
        public async Task<IActionResult> ToggleStatus([FromRoute] string id)
        {
            var result=await _roleServices.ToggleStatusAsync(id);
            return result.IsSuccess?NoContent():result.ToProblem(StatusCodes.Status404NotFound);
        }
    }
}