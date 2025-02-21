using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Plant_Project.API.Abstraction;
using Plant_Project.API.Authentication;
using Plant_Project.API.contracts.Authentication;
using Plant_Project.API.Services;

namespace Plant_Project.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IAuthServices authServices,IOptions<JwtOptions> Jwtoptions) : ControllerBase
    {
        private readonly IAuthServices _authServices = authServices;
        private readonly JwtOptions _Jwtoptions = Jwtoptions.Value;


        //https://localhost:7286/Auth
        [HttpPost("")]
        public async Task<IActionResult> LoginAcync(LoginRequestDTO Request, CancellationToken cancellationToken)
        {
            var result = await _authServices.GetTokenaync(Request.Email, Request.Password, cancellationToken);

            return result.IsSuccess ? Ok(result) : result.ToProblem(StatusCodes.Status404NotFound);
        }
        //https://localhost:7286/Auth/Refresh

        [HttpPost("Refresh")]
        public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var result = await _authServices.GetRefeshTokenaync(request.Token, request.RefreshToken, cancellationToken);

            return result.IsSuccess ?
                Ok(result) :
                result.ToProblem(StatusCodes.Status400BadRequest);

        }

        //https://localhost:7286/Auth/RevokeToken
        [HttpPost("RevokeToken")]
        public async Task<IActionResult> RevokeRefreshAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var result = await _authServices.RevokeRefeshTokenaync(request.Token, request.RefreshToken, cancellationToken);

            return result.IsSuccess ?
                Ok() :
                result.ToProblem(StatusCodes.Status400BadRequest);
        }
        //https://localhost:7286/Auth/Register
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterRequestDTO Request,CancellationToken cancellationToken)
        {
            var result = await _authServices.RegisterAsync(Request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem(StatusCodes.Status400BadRequest);
        }
    }
}
