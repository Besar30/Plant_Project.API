namespace Plant_Project.API.Abstraction.Consts
{
    public static class Permissions
    {
        public static string Type { get; } = "Permissions";

        // name of endpoint


        // method to get all permissions
        public static IList<string?> GetAllPermissions()=>
            typeof(Permissions).GetFields().Select(x=>x.GetValue(x) as string).ToList();

    }
}
