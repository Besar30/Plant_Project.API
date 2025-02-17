using Plant_Project.API.Abstraction;

namespace Plant_Project.API.Errors
{
    public static class CategoryError
    {
        public static readonly Error CategoryNotFound =
            new Error("Category Not Found", "Category Not Found");

        public static readonly Error CategoryDublicated=
           new Error("Category Dublicated", "Category already Exist");


    }
}
