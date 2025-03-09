using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
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
            user.PhoneNumber=updateProfileRequest.PhoneNumber;
            if (updateProfileRequest.ImagePath != null && updateProfileRequest.ImagePath.FileName != user.ImagePath)
            {
                if (!string.IsNullOrEmpty(user.ImagePath))
                {
                    DeleteImage(user.ImagePath);
                }
                string imagePath = await SaveImageAsync(updateProfileRequest.ImagePath); // تعديل هنا
                user.ImagePath = imagePath;
            }
            else if(user.ImagePath!=null && updateProfileRequest.ImagePath == null)
            {
                DeleteImage(user.ImagePath);
                user.ImagePath = "";
            }
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

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return "/images/" + uniqueFileName;
        }
        private void DeleteImage(string imagePath)
        {
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath.TrimStart('/'));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}
