namespace Plant_Project.API.contracts.Authentication
{
    public class ResetPasswordValidator:AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordValidator() {
            RuleFor(x => x.Password).NotEmpty();

             RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .Equal(x => x.Password).WithMessage("Passwords do not match.");
            
        }
    }
}
