
using Plant_Project.API.contracts.notification;
using Plant_Project.API.contracts.Posts;

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
                p.UserName!,
                p.Id
            )).ToList();
            return Result.Success(response);
        }

        public async Task<Result<PostResponse>> GetPostInNotification(int PostId, int NotificationId)
        {
            var p = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Reacts)
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(x => x.Id == PostId); 
            if (p == null)
            {
                return Result.Failure<PostResponse>(PostErrors.PostNotFound);
            }
            var notification = await _context.Notifications.FindAsync(NotificationId);
            notification.IsRead=true;
            await _context.SaveChangesAsync();
            var result = new PostResponse(
               p.Id,
                    p.Content,
                    string.IsNullOrEmpty(p.ImagePath)
                        ? null
                        : $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{p.ImagePath}",
                    p.User.UserName!,
                    string.IsNullOrEmpty(p.User.ImagePath)
                        ? null
                        : $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{p.User.ImagePath}",
                    p.Reacts.Count,
                    FormatDateFacebookStyle(p.CreatedAt),
                    p.Comments.Count

                );
            return Result.Success(result);
        }
        public static string FormatDateFacebookStyle(DateTime utcDateTime)
        {
            var egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, egyptTimeZone);
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptTimeZone);
            var timeSpan = now - localTime;

            if (timeSpan.TotalSeconds < 60)
                return "منذ ثوانٍ";
            if (timeSpan.TotalMinutes < 60)
                return $"منذ {(int)timeSpan.TotalMinutes} دقيقة";
            if (timeSpan.TotalHours < 24)
                return $"منذ {(int)timeSpan.TotalHours} ساعة";
            if (timeSpan.TotalDays < 2)
                return "أمس";
            if (timeSpan.TotalDays < 7)
                return $"منذ {(int)timeSpan.TotalDays} أيام";
            if (timeSpan.TotalDays < 30)
                return $"منذ {(int)(timeSpan.TotalDays / 7)} أسبوع";
            if (timeSpan.TotalDays < 365)
                return $"منذ {(int)(timeSpan.TotalDays / 30)} شهر";
            return $"منذ {(int)(timeSpan.TotalDays / 365)} سنة";
        }
    }
    
}
