using Microsoft.AspNetCore.Mvc;
using Plant_Project.API.contracts.Posts;
using System.Collections.Generic;

namespace Plant_Project.API.Services
{
    public class PostServices (ApplicationDbContext context, IHttpContextAccessor httpContextAccessor,IcacheService icacheService,ILogger<PostServices> logger, UserManager<ApplicationUser> userManager) : IPostServices
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IcacheService _icacheService = icacheService;
        private readonly ILogger<PostServices> _logger = logger;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        private const string _cachePerfix = "availablePost";

        public async Task<Result<List<PostResponse>>> GetAll(CancellationToken cancellationToken)
        {
            var cacheKey = $"{_cachePerfix}_all";
            var result = await _icacheService.GetAsync<List<PostResponse>> (cacheKey,cancellationToken);
            if (result is not null) {
                _logger.LogInformation("Get By cache");
               return Result.Success(result);
            }
            _logger.LogInformation("Get By Database");
            var posts = await _context.Posts
                            .Include(p => p.User)
                            .Include(x=>x.Reacts)
                            .OrderByDescending(p => p.CreatedAt)
                            .ToListAsync(cancellationToken);
              result = posts.Select(p => new PostResponse
             (
                    p.Id,
                    p.Content,
                   $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{p.ImagePath}",
                    p.User.UserName!,
                    $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{p.User.ImagePath}",
                    p.Reacts.Count
            )).ToList();

            await _icacheService.SetAsync(_cachePerfix,result,cancellationToken);
             return Result.Success(result);
        }

        public async Task<Result> AddPost([FromForm] PostRequestDTO post,string UserId, CancellationToken cancellationToken)
        {
            var Post=new Post();
            if (post.Content == null && post.ImagePath == null)
            {
                return Result.Failure(PostErrors.NullPost);
            }
            if (post.ImagePath != null) {
                string imagePath = await SaveImageAsync(post.ImagePath);
               // var absUri = $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{imagePath}";
                Post.ImagePath= imagePath;
            }
            if(post.Content!=null)
            Post.Content = post.Content;
            Post.UserId = UserId;
             _context.Add(Post);
            await _context.SaveChangesAsync(cancellationToken);
            var cacheKey = $"{_cachePerfix}_all";
            await _icacheService.RemoveAsync(cacheKey,cancellationToken);
            return Result.Success();
        }
        public async Task<Result<PostResponse>> GetById(int Id)
        {
            var cacheKey = $"{_cachePerfix}-{Id}";
            var response= await _icacheService.GetAsync<PostResponse>(cacheKey);
            if(response is not null)
            {
                _logger.LogInformation("Get By Cache");
                return Result.Success(response);
            }
            _logger.LogInformation("Get By Database");
            var post = await _context.Posts
                        .Include(p => p.User) 
                        .Include(x=>x.Reacts)
                        .FirstOrDefaultAsync(x => x.Id == Id);

            if (post == null)
                return Result.Failure<PostResponse>(PostErrors.PostNotFound);
             response = new PostResponse(
                post.Id,
                post.Content,
               $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{post.ImagePath}",
                post.User.UserName!,
                post.User.ImagePath,
                post.Reacts.Count
            );
            await _icacheService.SetAsync(cacheKey, response);
            return Result.Success(response);
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
    }
}