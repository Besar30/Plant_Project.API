namespace Plant_Project.API.Errors
{
    public static class PlantErrors
    {
        public static readonly Error PlantNotFound =
          new Error("Plant Not Found", "Plant Not Found");

        public static readonly Error PlantDublicated =
           new Error("Plant Dublicated", "Plant already Exist");
    }
}
