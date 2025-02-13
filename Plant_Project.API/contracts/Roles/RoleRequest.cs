namespace Plant_Project.API.Contracts.Roles;

public record RoleRequest(
    string Name,
    IList<string> Permissions
);