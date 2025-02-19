namespace Plant_Project.API.contracts.Plants;

public class PlantsRequestValidator :AbstractValidator<PlantsRequest>
{
	public PlantsRequestValidator()
	{
		RuleFor(x => x.Name).NotNull();
		RuleFor(x => x.Name)
			.NotEmpty()
			.Length(3, 100);

		RuleFor(x => x.Quantity).GreaterThan(-1).
				WithMessage("Quantity Must Greater Than 0");
	}

}
