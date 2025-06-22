using Microsoft.AspNetCore.Mvc;
using Plant_Project.API.contracts.Posts;
using System.Collections.Generic;

namespace Plant_Project.API.Services
{
    public class PostServices(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<PostServices> logger, UserManager<ApplicationUser> userManager) : IPostServices
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ILogger<PostServices> _logger = logger;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<Result<List<PostResponse>>> GetAll(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get From Database");
            var posts = await _context.Posts
                            .Include(p => p.User)
                            .Include(x => x.Reacts)
                            .Include(x => x.Comments)
                            .OrderByDescending(p => p.CreatedAt)
                            .ToListAsync(cancellationToken);

            var result = posts.Select(p => new PostResponse
            (
                p.Id,
                p.Content,
                $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{p.ImagePath}",
                p.User.UserName!,
                $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{p.User.ImagePath}",
                p.Reacts.Count,
                FormatDateFacebookStyle(p.CreatedAt),
                p.Comments.Count
            )).ToList();

            return Result.Success(result);
        }

        public async Task<Result> AddPost([FromForm] PostRequestDTO post, string UserId, CancellationToken cancellationToken)
        {
            var Post = new Post();

            if (post.Content == null && post.ImagePath == null)
            {
                return Result.Failure(PostErrors.NullPost);
            }

            if (post.ImagePath != null)
            {
                string imagePath = await SaveImageAsync(post.ImagePath);
                Post.ImagePath = imagePath;
            }

            if (post.Content != null)
                Post.Content = post.Content;

            Post.UserId = UserId;
            _context.Add(Post);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result<PostResponse>> GetById(int Id)
        {
            _logger.LogInformation("Get From Database");
            var post = await _context.Posts
                        .Include(p => p.User)
                        .Include(x => x.Reacts)
                        .Include(x => x.Comments)
                        .FirstOrDefaultAsync(x => x.Id == Id);

            if (post == null)
                return Result.Failure<PostResponse>(PostErrors.PostNotFound);

            var response = new PostResponse(
                post.Id,
                post.Content,
                $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{post.ImagePath}",
                post.User.UserName!,
               $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{post.User.ImagePath}",
                post.Reacts.Count,
                FormatDateFacebookStyle(post.CreatedAt),
                post.Comments.Count
            );

            return Result.Success(response);
        }

        public async Task<Result> DeletePost(int PostId, string UserId)
        {
            var post = await _context.Posts
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(x => x.Id == PostId);

            if (post == null)
            {
                return Result.Failure(PostErrors.PostNotFound);
            }

            var Id = post.User.Id;
            var user = await _context.Users.FindAsync(UserId);
            var isAdmin = await _userManager.IsInRoleAsync(user!, "Admin");

            if (Id == UserId || isAdmin)
            {
                _context.Remove(post);
                await _context.SaveChangesAsync();
                return Result.Success();
            }

            return Result.Failure(PostErrors.PostCanNotBeDeleted);
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

        public static string FormatDateFacebookStyle(DateTime utcDateTime)
        {
            var egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, egyptTimeZone);
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptTimeZone);
            var timeSpan = now - localTime;

            if (timeSpan.TotalSeconds < 60)
                return "منذ ثوانٍ";
            if (timeSpan.TotalMinutes < 60)
                return $"منذ {(int)timeSpan.TotalMinutes} دقيقة";
            if (timeSpan.TotalHours < 24)
                return $"منذ {(int)timeSpan.TotalHours} ساعة";
            if (timeSpan.TotalDays < 2)
                return "أمس";
            if (timeSpan.TotalDays < 7)
                return $"منذ {(int)timeSpan.TotalDays} أيام";
            if (timeSpan.TotalDays < 30)
                return $"منذ {(int)(timeSpan.TotalDays / 7)} أسبوع";
            if (timeSpan.TotalDays < 365)
                return $"منذ {(int)(timeSpan.TotalDays / 30)} شهر";
            return $"منذ {(int)(timeSpan.TotalDays / 365)} سنة";
        }
    }
}
