namespace Plant_Project.API.contracts.Authentication
{
    public record RefreshTokenRequest

    (
    string Token,
    string RefreshToken
        );
}
