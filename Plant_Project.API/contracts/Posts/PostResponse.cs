namespace Plant_Project.API.contracts.Posts
{
    public record PostResponse(
        int Id,
        string Content,
        string ImagePath,
        string UserName,
        string ImagePathUser,
        int ReactCount,
        string Time,
        int countComment
        );
    
}
