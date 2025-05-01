using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plant_Project.API.contracts.React;
using Plant_Project.API.Extensions;

namespace Plant_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReactController(IReactServices reactServices) : ControllerBase
    {
        private readonly IReactServices _reactServices = reactServices;

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetAllUserReacted([FromRoute]int postId) {
            var result = await _reactServices.GetAllUserReacted(postId);
            return result.IsSuccess ?
                Ok(result) : result.ToProblem(StatusCodes.Status404NotFound); 
        }
        [HttpPost("")]
        public async Task<IActionResult> AddReact([FromBody]ReactRequest request)
        {
            var result =await _reactServices.AddReactAsync(request,User.GetUserId()!);
            return result.IsSuccess ?
                Ok(result) : result.ToProblem(StatusCodes.Status400BadRequest);
        }
    }
}
