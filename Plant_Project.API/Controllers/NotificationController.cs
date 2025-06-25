using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plant_Project.API.Extensions;

namespace Plant_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController(INotificationService notificationService) : ControllerBase
    {
        private readonly INotificationService _notificationService = notificationService;

        [HttpGet("")]
        public async Task<IActionResult> GetNotification()
        {
            var result = await _notificationService.GetNotificationToUser(User.GetUserId()!);
            return result.IsSuccess? 
                Ok(result): result.ToProblem(StatusCodes.Status400BadRequest);
        }
        [HttpGet("post/{postId}/notification/{notificationId}")]
        public async Task<IActionResult> GetPostByNotification([FromRoute] int postId, [FromRoute] int notificationId)
        { 
            var resut = await _notificationService.GetPostInNotification(postId, notificationId);
            return resut.IsSuccess?
                Ok(resut):resut.ToProblem(StatusCodes.Status400BadRequest);
        }
    }
}
