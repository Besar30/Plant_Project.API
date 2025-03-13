using Plant_Project.API.contracts;
using Plant_Project.API.contracts.Comments;
using Plant_Project.API.Entities;
using System.Collections.Generic;
using System.Threading;

namespace Plant_Project.API.Services
{
    public class CommentServices(ApplicationDbContext context):ICommentServices
    {
        private readonly ApplicationDbContext _context = context;
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
            return Result.Success();
        }

        public async Task<Result<List<CommentResponse>>> GetCommentsByPost(int PostId)
        {
            var comments = await _context.Comments
                   .Where(x => x.PostId == PostId)
                   .Include(c => c.User) 
                   .OrderByDescending(c => c.CreatedAt)
                   .ToListAsync();
            var commentsResponse = comments.Select(c=>new CommentResponse(
                c.Id,
                 c.Content,
                 c.User.UserName!,
                 c.User.ImagePath
                )).ToList();
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
                return Result.Success();
            }
            if (comment.Post != null && comment.Post.UserId == userId)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success();
            }
            return Result.Failure(CommentErrors.CommentCanNotBeDeleted);
        }
    }
}
