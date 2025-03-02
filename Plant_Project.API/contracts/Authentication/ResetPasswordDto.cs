namespace Plant_Project.API.contracts.Authentication
{
    public record ResetPasswordDto(
        string Password,
        string ConfirmPassword,
        string Email,
        string Token
        );
}
