using FluentValidation;

namespace Plant_Project.API.contracts.Authentication
{
    public class ComfirmEamilRequestValidation:AbstractValidator<ComfirmEamilRequest>
    {
        public ComfirmEamilRequestValidation() { 
            RuleFor(x=>x.UserId).NotEmpty();
            RuleFor(x=>x.Code).NotEmpty();
        
            
        }
    }
}
