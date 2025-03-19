using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plant_Project.API.contracts.Posts;
using Plant_Project.API.Extensions;

namespace Plant_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostsController(IPostServices postServices) : ControllerBase
    {
        private readonly IPostServices _postServices = postServices;
      //  [Authorize(Roles =DefaultRoles.Member)]
        [HttpGet("")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var Posts= await _postServices.GetAll(cancellationToken);
            return Ok(Posts);
        }
       // [Authorize(Roles = DefaultRoles.Member)]
        [HttpPost("")]
        public async Task<IActionResult>AddPostAsync([FromForm]PostRequestDTO post,CancellationToken cancellationToken)
        {
            var result=await _postServices.AddPost(post,User.GetUserId()!,cancellationToken);


            return result.IsSuccess ?
               Ok(result) : result.ToProblem(StatusCodes.Status400BadRequest);
        }
       // [Authorize(Roles = DefaultRoles.Member)]
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById([FromRoute]int Id)
        {
            var result = await _postServices.GetById(Id);
            return result.IsSuccess?
                Ok(result):result.ToProblem(StatusCodes.Status400BadRequest);
        }
    }
}