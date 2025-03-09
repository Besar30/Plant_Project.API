using Plant_Project.API.Abstraction.Const.Pattern;

namespace Plant_Project.API.Contracts.Authentication;

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .Matches(RegexPatterns.Password)
            .WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase");

		RuleFor(x => x.ConfirmPassword)
		   .NotEmpty()
		   .Equal(x => x.NewPassword).WithMessage("Passwords do not match.");

	}
}