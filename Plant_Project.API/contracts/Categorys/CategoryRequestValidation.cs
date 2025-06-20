using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Plant_Project.API.contracts.Categorys
{
    public class CategoryRequestValidation: AbstractValidator<CategoryRequest>
    {
        private readonly ApplicationDbContext _context;

        public CategoryRequestValidation(ApplicationDbContext context) {

            _context=context;

            RuleFor(x => x.Name).NotNull();
            RuleFor(x => x.Name).NotEmpty()
                .Length(3, 100);
            RuleFor(x=>x.Description).NotNull();
            RuleFor(x=>x.Description).NotEmpty()
                .Length(3, 2000);
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
