namespace Plant_Project.API.contracts.Comments
{
    public class CommentValidator:AbstractValidator<CommentRequest>
    {
        public CommentValidator() { 
           RuleFor(x=>x.Content).NotEmpty().MinimumLength(1);
        }
    }
}
