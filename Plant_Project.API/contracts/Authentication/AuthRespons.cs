namespace Plant_Project.API.contracts.Authentication
{
    public record AuthRespons(
        string Id,
        string? Email,
        string FristName,
        string LastName,
        string Token,
		DateTimeOffset ExpirestIn,
        string RefreshToken,
        DateTime RefreshTokenExpiration
        );
}
