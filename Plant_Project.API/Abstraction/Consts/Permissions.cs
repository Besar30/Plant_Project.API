namespace Plant_Project.API.Abstraction.Consts
{
    public static class Permissions
    {
        public static string Type { get; } = "Permissions";

        // name of endpoint
        public const string GetPlant = "Plant:read";
        public const string AddPlant = "Plant:Add";
        public const string Deleteplant = "Plant:Delete";
        public const string Updateplant = "Plant:Update";


        public const string GetCategory = "Category:read";
        public const string AddCategory = "Category:Add";
        public const string DeleteCategory = "Category:Delete";
        public const string UpdaeCategory = "Category:Update";

        public const string GetUsers = "Users:read";
        public const string AddUsers = "Users:Add";
        public const string UpdateUsers = "Users:Update";

        public const string GetRoles = "Roles:read";
        public const string AddRoles = "Roles:Add";
        public const string UpdateRoles = "Roles:Update";

        public const string GetResults = "results:read";


        // method to get all permissions
        //public static IList<string?> GetAllPermissions()=>
        //    typeof(Permissions).GetFields().Select(x=>x.GetValue(x) as string).ToList();
        
            public static IList<string> GetAllPermissions() => new List<string>
           {
        "Plant:read",
        "Plant:Add",
        "Plant:Delete",
        "Plant:Update",
        "Category:read",
        "Category:Add",
        "Category:Delete",
        "Category:Update",
        "Users:read",
         "Users:Add",
         "Users:Update",
          "Roles:read",
          "Roles:Add",
          "Roles:Update",
          "results:read"
        // أضف باقي الصلاحيات هنا
    };
        


    }
}
