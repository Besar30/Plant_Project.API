using Plant_Project.API.Contracts.Authentication;

namespace Plant_Project.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IAuthServices authServices,IOptions<JwtOptions> Jwtoptions) : ControllerBase
    {
        private readonly IAuthServices _authServices = authServices;
        private readonly JwtOptions _Jwtoptions = Jwtoptions.Value;



        [HttpPost("")]
        public async Task<IActionResult> LoginAcync(LoginRequestDTO Request, CancellationToken cancellationToken)
        {
            var result = await _authServices.GetTokenaync(Request.Email, Request.Password, cancellationToken);

            return result.IsSuccess ? Ok(result) : result.ToProblem();
        }


        [HttpPost("Refresh")]
        public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var result = await _authServices.GetRefeshTokenaync(request.Token, request.RefreshToken, cancellationToken);

            return result.IsSuccess ?
                Ok(result) :
                result.ToProblem();

        }


        [HttpPost("RevokeToken")]
        public async Task<IActionResult> RevokeRefreshAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var result = await _authServices.RevokeRefeshTokenaync(request.Token, request.RefreshToken, cancellationToken);

            return result.IsSuccess ?
                Ok() :
                result.ToProblem();
        }
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterRequest Request,CancellationToken cancellationToken)
        {
            var result = await _authServices.RegisterAsync(Request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
		[HttpPost("forget-password")]
		public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
		{
			var result = await _authServices.SendResetPasswordCodeAsync(request.Email);

			return result.IsSuccess ? Ok() : result.ToProblem();
		}

	
	}
}
