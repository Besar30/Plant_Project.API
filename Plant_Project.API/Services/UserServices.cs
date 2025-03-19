
namespace Plant_Project.API.Services
{
    public class UserServices (UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor,IcacheService icacheService,ILogger<UserServices> ilogger) : IUserServices
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IcacheService _icacheService = icacheService;
        private readonly ILogger<UserServices> _ilogger = ilogger;
        private const string _cachePerfix = "availableUser";
        public async Task<Result<UserProfileResponse>> GetProfileAsync(string UserId)
        {
            var cacheKey = $"{_cachePerfix}_all";
            var User = await _icacheService.GetAsync<UserProfileResponse>(cacheKey);
            if(User is not null)
            {
                _ilogger.LogInformation("cache get by cache");
                return Result.Success(User);
            }
            _ilogger.LogInformation("cache get by Database");
            var result = await _userManager.Users.Where(x=>x.Id == UserId).SingleAsync();
            User = new UserProfileResponse(
                result.UserName!,
                result.Email!,
                result.FirstName,
                result.LastName,
                 $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{result.ImagePath}",
                result.PhoneNumber!
                );
            await _icacheService.SetAsync(cacheKey, User);
            return Result.Success(User);
        }

        public async Task<Result> UpdateProfileAsync(string UserId, UpdateProfileRequest updateProfileRequest)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
                return Result.Failure(UeserError.EmailNotFound);

            user.FirstName = updateProfileRequest.FirstName;
            user.LastName = updateProfileRequest.LastName;
            user.PhoneNumber = updateProfileRequest.PhoneNumber;

            if (updateProfileRequest.ImagePath != null && updateProfileRequest.ImagePath.FileName != user.ImagePath)
            {
                if (!string.IsNullOrEmpty(user.ImagePath))
                {
                    DeleteImage(user.ImagePath);
                }

                string imagePath = await SaveImageAsync(updateProfileRequest.ImagePath);

                // حفظ المسار الكامل في قاعدة البيانات
             //   var absUri = $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{imagePath}";
                user.ImagePath = imagePath;
            }
            else if (user.ImagePath != null && updateProfileRequest.ImagePath == null)
            {
                DeleteImage(user.ImagePath);
                user.ImagePath = "";
            }

            await _userManager.UpdateAsync(user);
            var cacheKey = $"{_cachePerfix}_all";
            await _icacheService.RemoveAsync(cacheKey);

            return Result.Success(user.Adapt<UserProfileResponse>());
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
