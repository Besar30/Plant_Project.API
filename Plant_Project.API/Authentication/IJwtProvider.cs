namespace Plant_Project.API.Authentication
{
    public interface IJwtProvider
    {
        (string token, int expiresIn) GenerateToken(ApplicationUser user);
        public string? validationToken(string token);
    }
}
