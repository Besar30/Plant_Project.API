namespace Plant_Project.API.contracts.Comments
{
    public record CommentResponse(
        int id,
        string Content,
        string UserName,
        string ImagePathUser
        );
}
