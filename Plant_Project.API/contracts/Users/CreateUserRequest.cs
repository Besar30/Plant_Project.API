namespace Plant_Project.API.Contracts.Users;

public record CreateUserRequest(
    string FirstName,
    string Email,
    string Password,
    IList<string> Roles
);