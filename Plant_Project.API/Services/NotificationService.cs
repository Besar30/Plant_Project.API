
using Plant_Project.API.contracts.notification;

namespace Plant_Project.API.Services
{
    public class NotificationService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : INotificationService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result<List<notificationResponse>>> GetNotificationToUser(string UserId)
        {
            var notifications = await _context.Notifications.Where(x => x.UserId == UserId).OrderByDescending(x=>x.CreatedAt).ToListAsync();

            var response = notifications.Select(p => new notificationResponse
            (
                p.Message!,
                p.IsRead,
                p.CreatedAt,
                p.PostId,
                $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{p.ImageUrl}",
                p.UserName!
            )).ToList();
            return Result.Success(response);
        }
    }
}