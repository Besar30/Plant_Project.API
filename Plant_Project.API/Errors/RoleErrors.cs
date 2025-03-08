namespace Plant_Project.API.Errors
{
    public static class RoleErrors
    {
        public static readonly Error RoleNotFound =
            new("Role.RoleNotFound", "Role is not found");

        public static readonly Error InvalidPermissions =
            new("Role.InvalidPermissions", "Invalid permissions");

        public static readonly Error DuplicatedRole =
            new("Role.DuplicatedRole", "Another role with the same name is already exists");
    }
}
