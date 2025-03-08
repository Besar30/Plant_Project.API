namespace Plant_Project.API.contracts.Roles
{
    public record RoleRequest(
      string Name,
      IList<string> Permissions
  );
}
