
using Plant_Project.API.contracts.Posts;
using System.Collections.Generic;

namespace Plant_Project.API.Services
{
    public class UserServices (ApplicationDbContext context,UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor,IcacheService icacheService,ILogger<UserServices> ilogger) : IUserServices
    {
        private readonly ApplicationDbContext _context = context;
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

            // فقط لو تم إرسال صورة جديدة ومختلفة عن الصورة الحالية
            if (updateProfileRequest.ImagePath != null && updateProfileRequest.ImagePath.FileName != user.ImagePath)
            {
                // حذف الصورة القديمة إن وجدت
                if (!string.IsNullOrEmpty(user.ImagePath))
                {
                    DeleteImage(user.ImagePath);
                }

                string imagePath = await SaveImageAsync(updateProfileRequest.ImagePath);
                user.ImagePath = imagePath;
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

        public async Task<Result<List<PostResponse>>> UserPost(string UserId,CancellationToken cancellationToken)
        {
           
            var userpost=await _context.Posts.Where(x=>x.UserId==UserId)
                                        .OrderByDescending(p => p.CreatedAt)
                                        .Include(p=>p.User)
                                       .ToListAsync(cancellationToken);
            var respons = userpost.Select(p => new PostResponse (
                  p.Id,
                  p.Content,
                  $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{p.ImagePath}",
                  p.User.UserName!,
                 $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{p.User.ImagePath}",
                 p.Reacts.Count()
                )).ToList();
            return Result.Success(respons);
        }

        public async Task<Result<List<UsersResponse>>> GetAllUser(string UserId)
        {
            var Users = await _context.Users.Where(x => x.Id != UserId).ToListAsync();
            var response = Users.Select(p => new UsersResponse(
                p.Id,
                p.Email!,
                p.UserName!,
                $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{p.ImagePath}"
                )).ToList();
            return Result.Success(response);
        }

        public async Task<Result> DeleteUser(string UserId)
        {
            var user = await _context.Users.FindAsync(UserId);
            if(user == null)
            {
                return Result.Failure(UeserError.UserNotFound);
            }
            var isAdmin = await _userManager.IsInRoleAsync(user!, "Admin");
            if (isAdmin) {
                return Result.Failure(UeserError.UserIsAdmin);
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Result.Success();
        }
    }
}
