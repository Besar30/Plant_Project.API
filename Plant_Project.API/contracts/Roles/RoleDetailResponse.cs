namespace Plant_Project.API.contracts.Roles
{
    public record RoleDetailResponse(
    string Id,
    string Name,
    bool IsDeleted,
    IEnumerable<string> Permissions
);
}
