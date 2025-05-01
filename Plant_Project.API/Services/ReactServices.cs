using Plant_Project.API.contracts.React;

namespace Plant_Project.API.Services
{
    public class ReactServices (ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : IReactServices
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result> AddReactAsync(ReactRequest reactRequest, string userId)
        {
            var reactDublicated= await _context.Reacts
                .Where(r=>r.UserId == userId && r.PostId==reactRequest.PostId).FirstOrDefaultAsync();
            if (reactDublicated != null) {
                _context.Reacts.Remove(reactDublicated!);
                await _context.SaveChangesAsync();
                return Result.Success("React removed.");
            }
            var react = new React
            {
                UserId = userId,
                PostId = reactRequest.PostId
            };
            _context.Reacts.Add(react);
            await _context.SaveChangesAsync();
            return Result.Success("React added.");
        }

        public async Task<Result<List<ReactResponse>>> GetAllUserReacted(int postId)
        {
            var post= await _context.Posts.Where(x=>x.Id==postId).FirstOrDefaultAsync();
            if (post == null)
            {
                return Result.Failure<List<ReactResponse>>(PostErrors.PostNotFound);
            }
            var User= await _context.Reacts.Where(x=>x.PostId==postId)
                                           .Include(r=>r.User).ToListAsync();
            var result= User.Select(x=> new ReactResponse(
                           x.UserId!,
                           x.User.UserName!,
                           $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{x.User.ImagePath}"
                )).ToList();
            return Result.Success(result);
        }
    }
}
