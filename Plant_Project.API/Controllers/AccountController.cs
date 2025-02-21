using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plant_Project.API.Abstraction;
using Plant_Project.API.contracts.Users;
using Plant_Project.API.Extensions;
using Plant_Project.API.Services;

namespace Plant_Project.API.Controllers
{
    [Route("me")]
    [ApiController]
    [Authorize]
    public class AccountController(IUserServices userServices) : ControllerBase
    {
        private readonly IUserServices _userServices = userServices;
        //https://localhost:7286/me
        [HttpGet("")]
        public async Task<IActionResult> Info()
        {
            var result = await _userServices.GetProfileAsync(User.GetUserId()!);
            return Ok(result.Value);
        }
        //https://localhost:7286/me/info
        [HttpPut("info")]
        public async Task<IActionResult> Info([FromBody] UpdateProfileRequest request)
        {
            await _userServices.UpdateProfileAsync(User.GetUserId()!, request);  
            return NoContent();
        }

        //https://localhost:7286/me/change-Password
        [HttpPut("change-Password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
           var result= await _userServices.ChangePasswordAsync(User.GetUserId()!, request);
            return result.IsSuccess ?
                NoContent() :
                result.ToProblem(StatusCodes.Status400BadRequest);
        }
    }
}
