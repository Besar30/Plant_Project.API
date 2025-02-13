namespace Plant_Project.API.Contracts.Users;

public record UserResponse(
    string Id,
    string FirstName,
    string Email,
    bool IsDisabled,
    IEnumerable<string> Roles
);