namespace Plant_Project.API.contracts.Authentication
{
    public record RegisterRequest(
         string UserName,
         string Email,
         string Password,
         string ComfirmPassword
        );
 
}
