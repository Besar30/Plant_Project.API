
using Plant_Project.API.Extensions;
namespace Plant_Project.API.Controllers
{
    [Route("me")]
    [ApiController]
    [Authorize]
    public class AccountController(IUserServices userServices,ApplicationDbContext context) : ControllerBase
    {
        private readonly IUserServices _userServices = userServices;
        private readonly ApplicationDbContext _context = context;

        //https://localhost:7286/me
        [HttpGet("")]
        public async Task<IActionResult> Info()
        {
            var result = await _userServices.GetProfileAsync(User.GetUserId()!);
            return Ok(result.Value);
        }
        //https://localhost:7286/me/info
        [HttpPut("info")]
        public async Task<IActionResult> Update([FromForm] UpdateProfileRequest request)
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
        //https://localhost:7286/me/Posts
        [HttpGet("Posts")]
        public async Task<IActionResult> UserPostAsync(CancellationToken cancellationToken)
        {
            var result= await _userServices.UserPost(User.GetUserId()!,cancellationToken);
            return result.IsSuccess ?
                   Ok(result.Value) :result.ToProblem(StatusCodes.Status400BadRequest);
        }
        [Authorize(Roles = DefaultRoles.Admin)]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userServices.GetAllUser(User.GetUserId()!);
            return Ok(result);
        }

        [Authorize(Roles = DefaultRoles.Admin)]
        [HttpDelete("{UserId}")]
        public async Task<IActionResult> DeleteUser([FromRoute]string UserId)
        {
            var result= await _userServices.DeleteUser(UserId);
            return result.IsSuccess ?
                Ok() : result.ToProblem(StatusCodes.Status400BadRequest);
        }
    }
}