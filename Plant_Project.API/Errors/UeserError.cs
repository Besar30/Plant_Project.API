using Plant_Project.API.Abstraction;

namespace Plant_Project.API.Errors
{
    public class UeserError
    {
		public static readonly Error InvalidCredentials =
		new("User.InvalidCredentials", "Invalid email/password", StatusCodes.Status401Unauthorized);

		public static readonly Error InvalidJwtToken =
			new("User.InvalidJwtToken", "Invalid Jwt token", StatusCodes.Status401Unauthorized);

		public static readonly Error InvalidRefreshToken =
			new("User.InvalidRefreshToken", "Invalid refresh token", StatusCodes.Status401Unauthorized);

		public static readonly Error DuplicatedEmail =
			new("User.DuplicatedEmail", "Another user with the same email is already exists", StatusCodes.Status409Conflict);

		public static readonly Error PasswordNotComfirmed =
            new("User.Password Not Comfirmed", "password not comfirmed", StatusCodes.Status400BadRequest);
    }
}
