using FluentValidation;
using Plant_Project.API.Const.Pattern;

namespace Plant_Project.API.contracts.Users
{
    public class ChangePasswordRequestValidation:AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidation() {
            RuleFor(x => x.CurrentPassword).NotEmpty();

            RuleFor(x=>x.NewPassword).NotEmpty()
                .Matches(RegexPattern.pattern).
                  WithMessage("Password should have LowerCase and UpperCase and Number and NonAlpapitic")
                  .NotEqual(x=>x.CurrentPassword)
                  .WithMessage("New Password cannot be same as the current password");
        }
    }
}
