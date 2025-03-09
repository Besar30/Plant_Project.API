
using Plant_Project.API.Contracts.Authentication;
using Plant_Project.API.Errors;

namespace Plant_Project.API.Services;

public class AuthServices(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IJwtProvider jwtProvider,
    ILogger<AuthServices> logger,
    IEmailSender emailSender,
    IHttpContextAccessor httpContextAccessor,
    ApplicationDbContext context
    ) : IAuthServices
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly int _refreshTokenExpiryDays = 14;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly ILogger<AuthServices> _logger = logger;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly ApplicationDbContext _context = context;
    public async Task<Result<AuthRespons>> GetTokenaync(string email, string password, CancellationToken cancellationToken = default)
    {
        //check email correct
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return Result.Failure<AuthRespons>(UserErrors.InvalidCredentials);

        if (user.IsDisabled)
            return Result.Failure<AuthRespons>(UserErrors.DisabledUser);
        // check password correct
        var result = await _signInManager.PasswordSignInAsync(user, password, false, true);
        if (result.Succeeded)
        {

            var (userRoles, userPermissions) = await GetUserRolesAndPermissions(user, cancellationToken);

            var (token, expiresIn) = _jwtProvider.GenerateToken(user, userRoles, userPermissions);
            var refreshToken = GenerateRefreshToken();
            var refreshTokenEXpirationDays = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                ExpiresOn = refreshTokenEXpirationDays
            });

            await _userManager.UpdateAsync(user);

			var expirationTime = DateTimeOffset.UtcNow.AddSeconds(expiresIn);

			// تمرير expirationTime بدلاً من expiresIn
			var resultt = new AuthRespons(
				user.Id,
				user.Email,
				user.FirstName,
				user.LastName,
				token,
				expirationTime,   // هنا تمرر DateTimeOffset بدلاً من expiresIn
				refreshToken,
				refreshTokenEXpirationDays
			);

			return Result.Success<AuthRespons>(resultt);
		}
		//401
		return Result.Failure<AuthRespons>(result.IsNotAllowed ? UserErrors.EmailNotConfirmed : UserErrors.InvalidCredentials);
	}
    

    public async Task<Result<AuthRespons>> GetRefeshTokenaync(string Token, string RefreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.validationToken(Token);

        if (userId is null)
            return Result.Failure<AuthRespons>(UserErrors.InvalidJwtToken);

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return Result.Failure<AuthRespons>(UserErrors.InvalidJwtToken);

        if (user.IsDisabled)
            return Result.Failure<AuthRespons>(UserErrors.DisabledUser);

        if (user.LockoutEnd > DateTime.UtcNow)
            return Result.Failure<AuthRespons>(UserErrors.LockedUser);

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == RefreshToken && x.IsActive);

        if (userRefreshToken == null)
            return Result.Failure<AuthRespons>(UserErrors.InvalidRefreshToken);

        userRefreshToken.RevokedOn = DateTime.UtcNow;
        var (userRoles, userPermissions) = await GetUserRolesAndPermissions(user, cancellationToken);


        var (Newtoken, expiresIn) = _jwtProvider.GenerateToken(user, userRoles, userPermissions);
        var NewrefreshToken = GenerateRefreshToken();
        var refreshTokenEXpirationDays = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = NewrefreshToken,
            ExpiresOn = refreshTokenEXpirationDays
        });

        await _userManager.UpdateAsync(user);
		var expirationTime = DateTimeOffset.UtcNow.AddSeconds(expiresIn);

		var resultt = new AuthRespons(
			user.Id,
			user.Email,
			user.FirstName,
			user.LastName,
			Newtoken,
			expirationTime,   
			RefreshToken,
			refreshTokenEXpirationDays
		);
		return Result.Success<AuthRespons>(resultt);
	}

    public async Task<Result> RevokeRefeshTokenaync(string Token, string RefreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.validationToken(Token);
        if (userId == null) return Result.Failure(UserErrors.InvalidJwtToken);
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return Result.Failure(UserErrors.InvalidJwtToken); ;
        var UserrefreshToken = user.RefreshTokens.FirstOrDefault(x => x.Token == RefreshToken && x.IsActive);
        if (UserrefreshToken == null) return Result.Failure(UserErrors.InvalidRefreshToken);
        UserrefreshToken.RevokedOn = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);
        return Result.Success();
    }

    public async Task<Result<AuthRespons>> RegisterAsync(RegisterRequest Request, CancellationToken cancellationToken = default)
    {

        var EmailIsExist = await _userManager.Users.AnyAsync(x => x.Email == Request.Email, cancellationToken);

        if (EmailIsExist)
            return Result.Failure<AuthRespons>(UserErrors.DuplicatedEmail);

        if (Request.Password != Request.ComfirmPassword)
            return Result.Failure<AuthRespons>(UserErrors.PasswordNotComfirmed);

        var user = new ApplicationUser
        {
            Email = Request.Email,
            UserName = Request.UserName,
            FirstName = Request.UserName
        };
        var result = await _userManager.CreateAsync(user, Request.Password);


        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, DefaultRoles.Member);

            var (userRoles, userPermissions) = await GetUserRolesAndPermissions(user, cancellationToken);
            var (token, expiresIn) = _jwtProvider.GenerateToken(user, userRoles, userPermissions);
            var refreshToken = GenerateRefreshToken();
            var refreshTokenEXpirationDays = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);
            user.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                ExpiresOn = refreshTokenEXpirationDays
            });
            await _userManager.UpdateAsync(user);
			var expirationTime = DateTimeOffset.UtcNow.AddSeconds(expiresIn);

			var resultt = new AuthRespons(
				user.Id,
				user.Email,
				user.FirstName,
				user.LastName,
				token,
				expirationTime,  
				refreshToken,
				refreshTokenEXpirationDays
			);
			return Result.Success<AuthRespons>(resultt);

		}
  
        var error = result.Errors.First();
        return Result.Failure<AuthRespons>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

    }

	public async Task<Result> ResetPassword(ResetPasswordRequest request)
	{
		var user = await _userManager.FindByEmailAsync(request.Email);
		if (user == null)
		{
			return Result.Failure(UserErrors.EmailNotFound);
		}
		var result = await _userManager.ResetPasswordAsync(user, request.Token!, request.NewPassword!);
		if (!result.Succeeded)
		{
			var error = result.Errors.First();
			return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
		}
		return Result.Success();
	}
	private string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    public async Task<Result> SendResetPasswordCodeAsync(string email)
    {
        if (await _userManager.FindByEmailAsync(email) is not { } user)
            return Result.Success();

        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        _logger.LogInformation("Reset code: {code}", code);

        await SendResetPasswordEmail(user, code);

        return Result.Success();

    }
    private async Task SendResetPasswordEmail(ApplicationUser user, string code)
    {
        var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

        var emailBody = EmailBodyBuilder.GenerateEmailBody("ForgetPassword",
            templateModel: new Dictionary<string, string>
            {
                { "{{name}}", user.FirstName },
                { "{{action_url}}", $"{origin}/auth/forgetPassword?email={user.Email}&code={code}" }
            }
        );

        BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "✅ PlantOpia: Change Password", emailBody));

        await Task.CompletedTask;
    }
    private async Task<(IEnumerable<string> roles, IEnumerable<string> permissions)> GetUserRolesAndPermissions(ApplicationUser user, CancellationToken cancellationToken)
    {
        var userRoles = await _userManager.GetRolesAsync(user);

        var userPermissions = await (from r in _context.Roles
                                     join p in _context.RoleClaims
                                     on r.Id equals p.RoleId
                                     where userRoles.Contains(r.Name!)
                                     select p.ClaimValue!)
                                     .Distinct()
                                     .ToListAsync(cancellationToken);

        return (userRoles, userPermissions);
    }
}


