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
           var posts= await _context.Posts.OrderByDescending(p=>p.CreatedAt).ToListAsync(cancellationToken);
            var result = posts.Adapt<List< PostResponse>>();
            return Result.Success<List<PostResponse >>(result);
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
            var post =  _context.Posts.Where(x=>x.Id == Id).FirstOrDefault();
            if (post == null)
                return Result.Failure<PostResponse>(PostErrors.PostNotFound);
            var response= post.Adapt<PostResponse>();
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