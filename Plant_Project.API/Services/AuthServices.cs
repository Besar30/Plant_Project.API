
using Microsoft.EntityFrameworkCore;

namespace Plant_Project.API.Services;

public class AuthServices(
    UserManager<ApplicationUser> userManager,
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
    private readonly ILogger<AuthServices> _logger = logger;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IEmailSender _emailSender = emailSender;
	private readonly ApplicationDbContext _context = context;
	public async Task<Result<AuthRespons>> GetTokenaync(string email, string password, CancellationToken cancellationToken = default)
    {
        //check email correct
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return Result.Failure<AuthRespons>(UeserError.InvalidCredentials);
        }
        // check password correct
        var isvalidpassword = await _userManager.CheckPasswordAsync(user, password);
        if (isvalidpassword == false)
        {
			return Result.Failure<AuthRespons>(UeserError.InvalidCredentials);

        }
		//generate jwt
      
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
        //return authrespons
        var result = new AuthRespons(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn, refreshToken, refreshTokenEXpirationDays);
        return Result.Success<AuthRespons>(result);
    }


    public async Task<Result<AuthRespons>> GetRefeshTokenaync(string Token, string RefreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.validationToken(Token);

        if (userId is null) 
            return Result.Failure<AuthRespons>(UeserError.InvalidJwtToken);

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null) 
            return Result.Failure<AuthRespons>(UeserError.InvalidJwtToken);

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == RefreshToken && x.IsActive);
        
        if (userRefreshToken == null) 
            return Result.Failure<AuthRespons>(UeserError.InvalidRefreshToken);
        
        userRefreshToken.RevokedOn = DateTime.UtcNow;
		var (userRoles, userPermissions) = await GetUserRolesAndPermissions(user, cancellationToken);


		var (Newtoken, expiresIn) = _jwtProvider.GenerateToken(user,userRoles,userPermissions);
        var NewrefreshToken = GenerateRefreshToken();
        var refreshTokenEXpirationDays = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);
        user.RefreshTokens.Add(new RefreshToken
        {
            Token = NewrefreshToken,
            ExpiresOn = refreshTokenEXpirationDays
        });
        await _userManager.UpdateAsync(user);
        var respons = new AuthRespons(user.Id, user.Email, user.FirstName, user.LastName, Newtoken, expiresIn, NewrefreshToken, refreshTokenEXpirationDays);
        return Result.Success<AuthRespons>(respons);
    }



    public async Task<Result> RevokeRefeshTokenaync(string Token, string RefreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.validationToken(Token);
        if (userId == null) return Result.Failure(UeserError.InvalidJwtToken);
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return Result.Failure(UeserError.InvalidJwtToken); ;
        var UserrefreshToken = user.RefreshTokens.FirstOrDefault(x => x.Token == RefreshToken && x.IsActive);
        if (UserrefreshToken == null) return Result.Failure(UeserError.InvalidRefreshToken);
        UserrefreshToken.RevokedOn = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);
        return Result.Success();
    }

    public async Task<Result<AuthRespons>> RegisterAsync(RegisterRequestDTO Request, CancellationToken cancellationToken = default)
    {

        var EmailIsExist = await _userManager.Users.AnyAsync(x => x.Email == Request.Email, cancellationToken);

        if (EmailIsExist)
            return Result.Failure<AuthRespons>(UeserError.DuplicatedEmail);

        if (Request.Password != Request.ComfirmPassword)
            return Result.Failure<AuthRespons>(UeserError.PasswordNotComfirmed);

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
            //return authrespons
            var resultt = new AuthRespons(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn, refreshToken, refreshTokenEXpirationDays);
            return Result.Success(resultt);

        }
        //badRequest
        var error = result.Errors.First();
        return Result.Failure<AuthRespons>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

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

		//var userPermissions = await _context.Roles
		//    .Join(_context.RoleClaims,
		//        role => role.Id,
		//        claim => claim.RoleId,
		//        (role, claim) => new { role, claim }
		//    )
		//    .Where(x => userRoles.Contains(x.role.Name!))
		//    .Select(x => x.claim.ClaimValue!)
		//    .Distinct()
		//    .ToListAsync(cancellationToken);

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


