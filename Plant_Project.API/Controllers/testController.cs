using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Plant_Project.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class testController : ControllerBase
    {
        [HttpGet("")]
        public IActionResult Get() {
            return Ok("adkamkancka");
        }
    }
}
