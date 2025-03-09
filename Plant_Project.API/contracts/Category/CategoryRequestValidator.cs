namespace Plant_Project.API.contracts.Category;

public class CategoryRequestValidation : AbstractValidator<CategoryRequest>
{
	private readonly ApplicationDbContext _context;

	public CategoryRequestValidation(ApplicationDbContext context)
	{

		_context = context;

		RuleFor(x => x.Name).NotNull();
		RuleFor(x => x.Name).NotEmpty()
			.Length(3, 100);
		RuleFor(x => x.Description).NotNull();
		RuleFor(x => x.Description).NotEmpty()
			.Length(3, 2000);
	}
}
