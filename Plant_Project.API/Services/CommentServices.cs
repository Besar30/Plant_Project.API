using Plant_Project.API.contracts;
using Plant_Project.API.contracts.Comments;
using Plant_Project.API.Entities;
using System.Collections.Generic;
namespace Plant_Project.API.Services
{
    public class CommentServices(ApplicationDbContext context,IcacheService icacheService,ILogger<CommentServices> logger):ICommentServices
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IcacheService _icacheService = icacheService;
        private readonly ILogger<CommentServices> _logger = logger;
        private const string _cachePerfix = "availableComment";

        public async Task<Result> AddComment(CommentRequest commentRequest, string userId,CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
            if (user == null) {
                return Result.Failure(UeserError.EmailNotFound);
            }
            var comment = new Comment
            {
                Content = commentRequest.Content,
                UserId = userId,
                PostId = commentRequest.PostId,
            };
            _context.Add(comment);
            await _context.SaveChangesAsync(cancellationToken);
            var cacheKey = $"{_cachePerfix}-{comment.PostId}";
            await _icacheService.RemoveAsync(cacheKey);
            return Result.Success();
        }
        public async Task<Result<List<CommentResponse>>> GetCommentsByPost(int PostId)
        {
            var cacheKey = $"{_cachePerfix}-{PostId}";
            var commentsResponse = await _icacheService.GetAsync<List<CommentResponse>>(cacheKey);
            if (commentsResponse is not null)
            {
                _logger.LogInformation("Get By cache");
                return Result.Success(commentsResponse);
            }
            _logger.LogInformation("Get By Database");
            var comments = await _context.Comments
                   .Where(x => x.PostId == PostId)
                   .Include(c => c.User) 
                   .OrderByDescending(c => c.CreatedAt)
                   .ToListAsync();
             commentsResponse = comments.Select(c=>new CommentResponse(
                 c.Id,
                 c.Content,
                 c.User.UserName!,
                 c.User.ImagePath
                )).ToList();
            await _icacheService.SetAsync(cacheKey, commentsResponse);
            return Result.Success(commentsResponse);
        }
        public async Task<Result> DeleteComment(int CommentId,string userId,CancellationToken cancellationToken)
        {
            var comment = await _context.Comments
                .Include(c => c.Post) 
                .FirstOrDefaultAsync(x => x.Id == CommentId);
            if (comment == null)
            {
                return Result.Failure(CommentErrors.CommentNotFound);
            }
            if (comment.UserId == userId)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync(cancellationToken);
                var cacheKey = $"{_cachePerfix}-{comment.Post.Id}";
                await _icacheService.RemoveAsync(cacheKey);
                return Result.Success();
            }
            if (comment.Post != null && comment.Post.UserId == userId)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync(cancellationToken);

                var cacheKey = $"{_cachePerfix}-{comment.Post.Id}";
                await _icacheService.RemoveAsync(cacheKey);

                return Result.Success();
            }
            return Result.Failure(CommentErrors.CommentCanNotBeDeleted);
        }
    }
}