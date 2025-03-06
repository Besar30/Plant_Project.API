namespace Plant_Project.API.Abstraction.Consts
{
    public static class Permissions
    {
        public static string Type { get; } = "Permissions";

        // name of endpoint
        public static string GetPlant = "Plant:read";
        public static string AddPlant = "Plant:Add";
        public static string Deleteplant = "Plant:Delete";
        public static string Updateplant = "Plant:Update";


        public static string GetCategory = "Category:read";
        public static string AddCategory = "Category:Add";
        public static string DeleteCategory = "Category:Delete";
        public static string UpdaeCategory = "Category:Update";

        public static string GetUsers = "Users:read";
        public static string AddUsers = "Users:Add";
        public static string UpdateUsers = "Users:Update";

        public static string GetRoles = "Roles:read";
        public static string AddRoles = "Roles:Add";
        public static string UpdateRoles = "Roles:Update";

        public static string GetResults = "results:read";


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
