using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("")]
        public async Task<IActionResult> Info()
        {
            var result = await _userServices.GetProfileAsync(User.GetUserId()!);
            return Ok(result.Value);
        }
    }
}
