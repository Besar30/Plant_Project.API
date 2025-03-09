namespace Plant_Project.API.Abstractions.Consts;

public static class Permissions
{
    public static string Type { get; } = "permissions";

    public const string GetPlant = "Plant:read";
    public const string AddPlant = "Plant:add";
    public const string UpdatePlant = "Plant:update";
    public const string DeletePlant = "Plant:delete";

	public const string GetCategory = "Category :read";
	public const string AddCategory = "Category :add";
	public const string UpdateCategory = "Category :update";
	public const string DeleteCategory = "Category :delete";

	public const string GetUsers = "users:read";
    public const string AddUsers = "users:add";
    public const string UpdateUsers = "users:update";

    public const string GetRoles = "roles:read";
    public const string AddRoles = "roles:add";
    public const string UpdateRoles = "roles:update";


    public static IList<string?> GetAllPermissions() =>
        typeof(Permissions).GetFields().Select(x => x.GetValue(x) as string).ToList();
}