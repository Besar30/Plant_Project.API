using FluentValidation;

namespace Plant_Project.API.contracts.Plants
{
    public class PlantsRequestValidation: AbstractValidator<PlantsRequest>
    {
        public PlantsRequestValidation() {
            RuleFor(x => x.Name).NotNull();
            RuleFor(x => x.Name).NotEmpty().
                Length(3, 100);
            RuleFor(x=>x.Description).NotNull();
            RuleFor(x=>x.Description).NotEmpty().
                Length(3, 2000);
            RuleFor(x=>x.How_To_Plant).NotNull();
            RuleFor(x=>x.How_To_Plant).NotEmpty().
                Length(3, 2000);
            RuleFor(x => x.Price).GreaterThan(0)
                .WithMessage("Price Must Greater Than 0");
            RuleFor(x => x.Quantity).GreaterThan(-1).
                WithMessage("Quantity Must Greater Than 0");
            RuleFor(p => p.CategoryId)
           .GreaterThan(0).WithMessage("Category Id Must Greater Than 0");
            //   RuleFor(x => x.ImagePath).NotNull();
            RuleFor(x => x.ImagePath)
                          .Must(BeAValidImage)
                          .When(x => x.ImagePath != null)
                          .WithMessage("File Allowed Extention{.jpg,.jpeg, .png}");
        }
        private bool BeAValidImage(IFormFile file)
        {
            if (file == null) return false;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            return allowedExtensions.Contains(fileExtension);
        }
    }

}
