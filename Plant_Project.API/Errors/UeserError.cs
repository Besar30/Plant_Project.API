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
        public static readonly Error DuplicateEmail =
            new("User.Email Duplicated", "This Email is already Used");
        public static readonly Error PasswordNotComfirmed =
            new("User.Password Not Comfirmed", "password not comfirmed");


        public static readonly Error EmailNotComfirmed =
           new("User.EmailNotComfirmed", "Email Not Comfirmed");

        public static readonly Error InvalidCode =
          new("User.InvalidCode", "Invalid Code");

        public static readonly Error DuplicatedConfirmation =
        new("User.DuplicatedConfirmation", "Email already confirmed");
        public static readonly Error EmailNotFound=
          new("User.EmailNotFound", "Email Not Found");


        public static readonly Error UserNotFound =
        new("User.UserNotFound", "User is not found");
        public static readonly Error UserIsAdmin =
       new("User.UserIsAdmin", "You Cant delete this email");
    }
}

