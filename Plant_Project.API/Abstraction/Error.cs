namespace Plant_Project.API.Abstraction
{
    public record Error(string Code, string Discription)
    {
        public static readonly Error None = new(string.Empty, string.Empty);
    }
}
