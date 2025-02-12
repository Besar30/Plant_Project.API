using Plant_Project.API.Abstraction.Const.Pattern;

namespace Plant_Project.API.contracts.Users;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
	public ChangePasswordRequestValidator()
	{
		RuleFor(x => x.CurrentPassword)
			.NotEmpty();

		RuleFor(x => x.NewPassword)
			.NotEmpty()
			.Matches(RegexPatterns.Password)
			.WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase")
			.NotEqual(x => x.CurrentPassword)
			.WithMessage("New password cannot be same as the current password");
	}
}