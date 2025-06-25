using Plant_Project.API.contracts.notification;
using Plant_Project.API.contracts.Posts;

namespace Plant_Project.API.Services
{
    public interface INotificationService
    {
        public Task<Result<List<notificationResponse>>> GetNotificationToUser(string UserId);
        public Task<Result<PostResponse>> GetPostInNotification(int PostId,int NotificationId);
    }
}
