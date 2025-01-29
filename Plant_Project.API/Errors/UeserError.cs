using Plant_Project.API.Abstraction;

namespace Plant_Project.API.Errors
{
    public class UeserError
    {
        public static readonly Error InvalidCerdentials =
             new(" User.InvalidCerdentials", "Invalid email/Password");

        public static readonly Error InvalidJwtToken =
           new("User.InvalidJwtToken", "Invalid Jwt token");

        public static readonly Error InvalidRefreshToken =
            new("User.InvalidRefreshToken", "Invalid refresh token");
    }
}
