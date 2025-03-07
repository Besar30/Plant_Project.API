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

        public async Task<Result> UpdateProfileAsync(string UserId, UpdateProfileRequest updateProfileRequest)
        {
            var user=await _userManager.FindByIdAsync(UserId);
            user!.FirstName=updateProfileRequest.FirstName;
            user.LastName=updateProfileRequest.LastName;
            await _userManager.UpdateAsync(user);
            return Result.Success();
        }
        public async Task<Result> ChangePasswordAsync(string UserId, ChangePasswordRequest request )
        {
            var user= await _userManager.FindByIdAsync(UserId);
            var result = await _userManager.ChangePasswordAsync(user!, request.CurrentPassword, request.NewPassword);
            if (result.Succeeded) { 
               return Result.Success();
            }
            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description));
        }
    }
}
