namespace Plant_Project.API.contracts.Authentication
{
    public record RegisterRequestDTO(
         string UserName,
         string Email,
         string Password,
         string ComfirmPassword
        );
}
