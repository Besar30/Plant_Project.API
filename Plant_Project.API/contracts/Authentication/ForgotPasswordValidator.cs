namespace Plant_Project.API.contracts.Authentication
{
    public class ForgotPasswordValidator:AbstractValidator<ForgotPasswordDto>
    {
        public ForgotPasswordValidator() {
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
            RuleFor(x=>x.ClientUri).NotEmpty();
        
        }
    }
}
