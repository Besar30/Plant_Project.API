using Plant_Project.API.contracts.Posts;
using System.Collections.Generic;

namespace Plant_Project.API.Services
{
    public class PostServices (ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : IPostServices
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result<List<PostResponse>>> GetAll(CancellationToken cancellationToken)
        {
               var posts = await _context.Posts
                            .Include(p => p.User)
                            .OrderByDescending(p => p.CreatedAt)
                            .ToListAsync(cancellationToken);

             var result = posts.Select(p => new PostResponse
             (
                    p.Id,
                    p.Content,
                    p.ImagePath,
                    p.User.UserName!,
                    p.User.ImagePath
            )).ToList();

             return Result.Success(result);
        }

        public async Task<Result> AddPost(PostRequestDTO post,string UserId, CancellationToken cancellationToken)
        {
            var Post=new Post();
            if (post.Content == null && post.ImagePath == null)
            {
                return Result.Failure(PostErrors.NullPost);
            }
            if (post.ImagePath != null) {
                string imagePath = await SaveImageAsync(post.ImagePath);
                var absUri = $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{imagePath}";
                Post.ImagePath=absUri;
            }
            if(post.Content!=null)
            Post.Content = post.Content;
            Post.UserId = UserId;
             _context.Add(Post);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        public async Task<Result<PostResponse>> GetById(int Id)
        {
            var post = await _context.Posts
                        .Include(p => p.User) 
                        .FirstOrDefaultAsync(x => x.Id == Id);

            if (post == null)
                return Result.Failure<PostResponse>(PostErrors.PostNotFound);
            var response = new PostResponse(
                post.Id,
                post.Content,
                post.ImagePath,
                post.User.UserName!,
                post.User.ImagePath
            );
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

       
    }
}