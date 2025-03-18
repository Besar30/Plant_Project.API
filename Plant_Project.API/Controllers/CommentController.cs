using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plant_Project.API.contracts;
using Plant_Project.API.Extensions;

namespace Plant_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController(ICommentServices commentServices) : ControllerBase
    {
        private readonly ICommentServices _commentServices = commentServices;

        [HttpGet("{postId}")]
        public async Task<IActionResult>GetAllCommentByPost([FromRoute]int postId)
        {
            var result = await _commentServices.GetCommentsByPost(postId);
            return Ok(result);
        }

        [HttpPost("")]
        public async Task<IActionResult> AddCommentAsync([FromBody]CommentRequest commentRequest,CancellationToken cancellationToken)
        {
            var result= await _commentServices.AddComment(commentRequest,User.GetUserId()!,cancellationToken);
            return result.IsSuccess ?
                Ok(result) : result.ToProblem(StatusCodes.Status404NotFound);
        }
        [HttpDelete("{CommentId}")]
        public async Task<IActionResult> DeleteComment([FromRoute]int CommentId,CancellationToken cancellationToken)
        {
            var resutl= await _commentServices.DeleteComment(CommentId,User.GetUserId()!,cancellationToken);
            return resutl.IsSuccess?
                Ok():resutl.ToProblem(StatusCodes.Status400BadRequest);
        }
    }
}
