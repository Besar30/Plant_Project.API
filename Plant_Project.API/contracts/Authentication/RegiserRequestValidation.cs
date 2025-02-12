using FluentValidation;
using Plant_Project.API.Abstraction.Const.Pattern;

namespace Plant_Project.API.contracts.Authentication
{
	public class RegiserRequestValidation:AbstractValidator<RegisterRequestDTO>
    {
        public RegiserRequestValidation() {
            RuleFor(x => x.UserName).NotEmpty().Length(3,100);
          RuleFor(x => x.Password).NotEmpty().Matches(RegexPatterns.Password).
                WithMessage("Password should have LowerCase and UpperCase and Number and NonAlpapitic");
            RuleFor(x => x.Email).NotEmpty()
                .EmailAddress();

            RuleFor(x => x.ComfirmPassword)
       .NotEmpty()
       .Equal(x => x.Password).WithMessage("Passwords do not match.");

        }
    }
}
