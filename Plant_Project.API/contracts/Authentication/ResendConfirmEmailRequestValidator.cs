namespace Plant_Project.API.contracts.Authentication;

public class ResendConfirmationEmailRequestValidator : AbstractValidator<ResendConfirmationEmailRequest>
{
	public ResendConfirmationEmailRequestValidator()
	{
		RuleFor(x => x.Email).NotEmpty().EmailAddress();


	}
}