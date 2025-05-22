using Microsoft.AspNetCore.SignalR;
using Plant_Project.API.Const.SignalR;
using Plant_Project.API.contracts;
using Plant_Project.API.contracts.Comments;
using Plant_Project.API.Entities;
using System.Collections.Generic;
namespace Plant_Project.API.Services
{
    public class CommentServices(ApplicationDbContext context,IcacheService icacheService,ILogger<CommentServices> logger, IHubContext<NotificationHub> notificationHub, UserManager<ApplicationUser> userManager) :ICommentServices
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IcacheService _icacheService = icacheService;
        private readonly ILogger<CommentServices> _logger = logger;
        private readonly IHubContext<NotificationHub> _notificationHub = notificationHub;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
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
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.Id == commentRequest.PostId, cancellationToken);
            if (post != null) {
              var ownerId=post.UserId;
                string notificationMessage = $"{user.UserName} commented on your post: {commentRequest.Content}. ";
                if (NotificationHub.userConnections.ContainsKey(ownerId)) {
                   
                    await _notificationHub.Clients.User(ownerId).SendAsync("ReceiveNotification", notificationMessage);

                }
                var notification = new Notification
                {
                    UserId = ownerId,
                    Message = notificationMessage,
                    IsRead = false,
                    PostId=commentRequest.PostId,
                    ImageUrl=user.ImagePath,
                    UserName= user.UserName
                };
                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync(cancellationToken);
            }
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
            var user = await _context.Users.FindAsync(userId);
            var isAdmin = await _userManager.IsInRoleAsync(user!, "Admin");
            if (comment.UserId == userId||isAdmin)
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