namespace Plant_Project.API.contracts.notification
{
    public record notificationResponse(
        string message,
        bool IsRead,
        DateTime CreatedAt,
        int PostId,
        string ImageUrl,
        string CommenterName
        );
    
}
