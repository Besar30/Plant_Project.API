using FluentValidation;
using Plant_Project.API.contracts.Authentication;

namespace Plant_Project.API.Contracts.Authentication
{
    public class AuthValidator : AbstractValidator<LoginRequestDTO>
    {
        public AuthValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Please provide a valid email address.")
                .MaximumLength(100).WithMessage("Email must not exceed 100 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
             
        }
    }
}
