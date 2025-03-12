namespace Plant_Project.API.contracts.Posts
{
    public class PostRequestDTOValidator:AbstractValidator<PostRequestDTO>
    {
        public PostRequestDTOValidator() { 

            RuleFor(x=>x).
                Must(x=>!string.IsNullOrWhiteSpace(x.Content)||x.ImagePath!=null)
                .WithMessage("You must provider either content or an image");


            RuleFor(x => x.ImagePath)
           .Must(BeAValidImageOrNull).WithMessage("File Allowed Extention{.jpg, .jpeg, .png}");


        }
        private bool BeAValidImageOrNull(IFormFile? file)
        {
            // إذا كانت الصورة غير موجودة (null) يعتبر التحقق صحيحًا
            if (file == null) return true;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            // تحقق من أن الامتداد مسموح به
            return allowedExtensions.Contains(fileExtension);
        }
    }
}
