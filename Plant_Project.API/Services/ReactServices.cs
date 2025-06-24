using Microsoft.AspNetCore.SignalR;
using Plant_Project.API.Const.SignalR;
using Plant_Project.API.contracts.React;

namespace Plant_Project.API.Services
{
    public class ReactServices (ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHubContext<NotificationHub> notificationHub) : IReactServices
    {

        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IHubContext<NotificationHub> _notificationHub = notificationHub;

        public async Task<Result> AddReactAsync(ReactRequest reactRequest, string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
                return Result.Failure(UeserError.EmailNotFound);
            if (reactRequest.PostId == null)
            {
                return Result.Failure(PostErrors.PostNotFound);
            }
            var existingReact = await _context.Reacts
                .FirstOrDefaultAsync(r => r.UserId == userId && r.PostId == reactRequest.PostId);
            if (existingReact != null)
            {
                _context.Reacts.Remove(existingReact);
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

            // 📢 إشعار لصاحب البوست
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.Id == reactRequest.PostId);
            if (post != null)
            {
                var ownerId = post.UserId;

                // ✅ نتأكد إنه مش بيرد على نفسه
                if (ownerId != userId)
                {
                    string notificationMessage = $"{user.UserName} liked your post.";

                    // Send real-time notification if online
                    if (NotificationHub.userConnections.ContainsKey(ownerId))
                    {
                        await _notificationHub.Clients.User(ownerId).SendAsync("ReceiveNotification", notificationMessage);
                    }

                    // Save notification in DB
                    var notification = new Notification
                    {
                        UserId = ownerId,
                        Message = notificationMessage,
                        IsRead = false,
                        PostId = reactRequest.PostId,
                        ImageUrl = user.ImagePath,
                        UserName = user.UserName
                    };

                    _context.Notifications.Add(notification);
                    await _context.SaveChangesAsync();
                }
            }

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
