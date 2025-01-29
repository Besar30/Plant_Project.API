using Microsoft.AspNetCore.Identity;
using Plant_Project.API.Abstraction;
using Plant_Project.API.Authentication;
using Plant_Project.API.contracts.Authentication;
using Plant_Project.API.Errors;
using System.Security.Cryptography;

namespace Plant_Project.API.Services
{
    public class AuthServices(UserManager<ApplicationUser> userManager,IJwtProvider jwtProvider) : IAuthServices
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        private readonly int _refreshTokenExpiryDays = 14;


        public async Task<Result<AuthRespons>>GetTokenaync(string email, string password, CancellationToken cancellationToken = default)
        {
            //check email correct
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) {
                return Result.Failure<AuthRespons>(UeserError.InvalidCerdentials);
            }
            // check password correct
            var isvalidpassword = await _userManager.CheckPasswordAsync(user, password);
            if (isvalidpassword == false)
            {
                return Result.Failure<AuthRespons>(UeserError.InvalidCerdentials);

            }
            //generate jwt
            var (token, expiresIn) = _jwtProvider.GenerateToken(user);
            var refreshToken = GenerateRefreshToken();
            var refreshTokenEXpirationDays = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);
            user.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                ExpiresOn = refreshTokenEXpirationDays
            });
            await _userManager.UpdateAsync(user);
            //return authrespons
            var result = new AuthRespons(user.Id,user.Email,user.FirstName,user.LastName,token,expiresIn,refreshToken,refreshTokenEXpirationDays);
            return Result.Success<AuthRespons>(result); 
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

            var (Newtoken, expiresIn) = _jwtProvider.GenerateToken(user);
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
        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }
}
