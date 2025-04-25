using Plant_Project.API.contracts.notification;

namespace Plant_Project.API.Services
{
    public interface INotificationService
    {
        public Task<Result<List<notificationResponse>>> GetNotificationToUser(string UserId);
    }
}
