namespace Plant_Project.API.Abstraction
{
    public record Error(string Code, string Discription,int? StatusCode)
    {
        public static readonly Error None = new(string.Empty, string.Empty,null);
    }
}
