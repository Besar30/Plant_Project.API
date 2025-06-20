namespace Plant_Project.API.contracts.Categorys
{
    public record CategoryRequest(
         string Name,
         string Description,
         IFormFile? ImagePath
        );
}