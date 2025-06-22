using Microsoft.AspNetCore.WebUtilities;
using Plant_Project.API.Abstraction.Consts;
using Plant_Project.API.contracts.Authentication;
using Plant_Project.API.Helpers;
using System.Security.Cryptography;
using System.Threading;
namespace Plant_Project.API.Services
{
    public class AuthServices(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager
        ,IJwtProvider jwtProvider,ILogger<AuthServices> logger,IEmailSender emailSender,IHttpContextAccessor httpContextAccessor,ApplicationDbContext context) : IAuthServices
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        private readonly ILogger<AuthServices> _logger = logger;
        private readonly IEmailSender _emailSender = emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ApplicationDbContext _context = context;
        private readonly int _refreshTokenExpiryDays = 14;
        public async Task<Result<AuthRespons>>GetTokenaync(string email, string password, CancellationToken cancellationToken = default)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null) {
                    return Result.Failure<AuthRespons>(UeserError.InvalidCerdentials);
                }
                var result= await _signInManager.PasswordSignInAsync(user,password, false,false);
                if (result.Succeeded)
                {
                var (userRoles, userPermissions) = await GetRolesAndPermissions(user, cancellationToken);
                    var (token, expiresIn) = _jwtProvider.GenerateToken(user,userRoles,userPermissions);
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
                    refreshTokenEXpirationDays,
                   $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{user.ImagePath}"
                );

                return Result.Success<AuthRespons>(resultt);

                }
            return Result.Failure<AuthRespons>(result.IsNotAllowed?UeserError.EmailNotComfirmed:UeserError.InvalidCerdentials);
            }
        public async Task<Result<AuthRespons>> GetRefeshTokenaync(string Token, string RefreshToken, CancellationToken cancellationToken = default)
        {
            var userId = _jwtProvider.validationToken(Token);
            if (userId is null) return Result.Failure<AuthRespons>(UeserError.InvalidJwtToken);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return Result.Failure<AuthRespons>(UeserError.InvalidJwtToken);
            var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == RefreshToken && x.IsActive);
            if (userRefreshToken == null) return Result.Failure<AuthRespons>(UeserError.InvalidRefreshToken);
            userRefreshToken.RevokedOn = DateTime.UtcNow;
            var (userRoles, userPermissions) = await GetRolesAndPermissions(user, cancellationToken);

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
                refreshTokenEXpirationDays,
                   $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{user.ImagePath}"
            );
            return Result.Success<AuthRespons>(resultt);
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
                return Result.Failure<AuthRespons>(UeserError.DuplicateEmail);

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
                var (userRoles, userPermissions) = await GetRolesAndPermissions(user, cancellationToken);
                var (token, expiresIn) = _jwtProvider.GenerateToken(user,userRoles,userPermissions);
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
                    refreshTokenEXpirationDays,
                   $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{user.ImagePath}"
                );
                return Result.Success<AuthRespons>(resultt);
                
            }
            var error = result.Errors.First();
            return Result.Failure<AuthRespons>(new Error(error.Code, error.Description));

        }
        //public async Task<Result> ConfirmEamilAsync(ComfirmEamilRequest Request)
        //{
        //    if(await _userManager.FindByIdAsync(Request.UserId) is not { } user)
        //        return Result.Failure(UeserError.InvalidCode);
        //    if (user.EmailConfirmed)
        //        return Result.Failure(UeserError.DuplicatedConfirmation);
        //    var code = Request.Code;
        //    try
        //    {
        //        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        //    }
        //    catch(FormatException)
        //    {
        //        return Result.Failure(UeserError.InvalidCode);

        //    }

        //    var result = await _userManager.ConfirmEmailAsync(user,code);
        //    if (result.Succeeded)
        //    {
        //        return Result.Success();
        //    }
        //    //badRequest
        //    var error = result.Errors.First();
        //    return Result.Failure(new Error(error.Code, error.Description));

        //}
        //public async Task<Result> ResendConfirmEamilAsync(ResendConfirmationEmailRequest Request)
        //{
        //    if (await _userManager.FindByEmailAsync(Request.Email) is not { } user)
        //        return Result.Success();
        //    if(user.EmailConfirmed)
        //        return Result.Failure(UeserError.DuplicatedConfirmation);


        //    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        //    _logger.LogInformation("Comfirmation Code :{code}", code);

        //    //TODO: send Email
        //    await SendConfirmation(user, code);

        //    return Result.Success();
        //}
        public async Task<Result<string>> ForgetPassword(ForgotPasswordDto forgotPassword)
        {
         
            var user= await _userManager.FindByEmailAsync(forgotPassword.Email);
            if (user == null) {
                return Result.Failure<string>(UeserError.EmailNotFound);
            }
            var token= await _userManager.GeneratePasswordResetTokenAsync(user);
            var param = new Dictionary<string, string>
            {
                {"token",token },
                {"email",forgotPassword.Email}
            };
            var callback=QueryHelpers.AddQueryString(forgotPassword.ClientUri, param);
           // var message = new Message([user.Email!], "Reset password token", callback, null);
            await _emailSender.SendEmailAsync(user.Email!, "Reset password token", callback);
            return Result.Success(token);

        }
        public async Task<Result> ResetPassword(ResetPasswordDto resetPassword)
        {
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null) {
                return Result.Failure(UeserError.EmailNotFound);
            }
            var result = await _userManager.ResetPasswordAsync(user, resetPassword.Token!, resetPassword.Password!);
            if (!result.Succeeded)
            {
                var error = result.Errors.First();
                return Result.Failure(new Error(error.Code, error.Description));
            }
            return Result.Success();
        }
        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
        private async Task SendConfirmation(ApplicationUser user,string code)
        {
            var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;
            var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfrmation",
                new Dictionary<string, string>
                {
                        {
                            "{{name}}",user.FirstName
                    },
                    {
                             "{{action_url}}", $"{origin}/auth/emailConfirmation?userId={user.Id}&code={code}"
                        }
                }
                );
            await _emailSender.SendEmailAsync(user.Email!, "Plant project : Email comfirmation", emailBody);
        }

       private async Task<(IEnumerable<string>roles,IEnumerable<string> permissions)> GetRolesAndPermissions(ApplicationUser user,CancellationToken cancellationToken)
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
}