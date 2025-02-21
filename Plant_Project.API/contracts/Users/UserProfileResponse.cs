namespace Plant_Project.API.contracts.Users
{
    public record UserProfileResponse(
        string UserName,
        string Email,
        String FirstName,
        string LastName
        );
}
