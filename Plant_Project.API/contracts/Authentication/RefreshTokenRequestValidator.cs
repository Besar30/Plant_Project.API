using FluentValidation;

namespace Plant_Project.API.contracts.Authentication
{
    public class RefreshTokenRequestValidator
   : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(x => x.Token).NotEmpty();
            RuleFor(x => x.RefreshToken).NotEmpty();
        }
    }
}
