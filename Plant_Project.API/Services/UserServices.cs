using Mapster;
using Microsoft.AspNetCore.Identity;
using Plant_Project.API.Abstraction;
using Plant_Project.API.contracts.Users;

namespace Plant_Project.API.Services
{
    public class UserServices (UserManager<ApplicationUser> userManager): IUserServices
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<Result<UserProfileResponse>> GetProfileAsync(string UserId)
        {
            var User = await _userManager.Users.Where(x => x.Id == UserId).
                ProjectToType<UserProfileResponse>().
                SingleAsync();
            return Result.Success(User);
        }
    }
}
