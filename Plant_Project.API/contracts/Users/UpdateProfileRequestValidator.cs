public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().Length(3, 100);
        RuleFor(x => x.LastName).NotEmpty().Length(3, 100);

        RuleFor(x => x.ImagePath)
            .Must(BeAValidImageOrNull).WithMessage("File Allowed Extention{.jpg, .jpeg, .png}");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^(\+?\d{1,3})?[-. ]?\d{10}$")
            .WithMessage("Invalid phone number format");
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
