namespace Plant_Project.API.contracts.Posts
{
    public record PostRequestDTO(
        string? Content,
        IFormFile? ImagePath
        );
}
