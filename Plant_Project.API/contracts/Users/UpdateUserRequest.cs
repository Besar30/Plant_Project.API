namespace Plant_Project.API.Contracts.Users;

public record UpdateUserRequest(
    string FirstName,

    string Email,
    IList<string> Roles
);