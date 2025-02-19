
using Plant_Project.API.Contracts.Roles;
using Plant_Project.API.Errors;

namespace Plant_Project.API.Services;

public class PlantServices(ApplicationDbContext context) :IPlantServices
{
	private readonly ApplicationDbContext _context = context;
	public async Task<IEnumerable<PlantsResponse>> GetAllAsync(CancellationToken cancellationToken = default) =>
	   await _context.Plants
		   .AsNoTracking()
	.ProjectToType<PlantsResponse>()
	.ToListAsync(cancellationToken);

	public async Task<Result<PlantsResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
	{
		var poll = await _context.Plants.FindAsync(id, cancellationToken);

		return poll is not null
		? Result.Success(poll.Adapt<PlantsResponse>())
		: Result.Failure<PlantsResponse>(PlantsErrors.PlantNotFound);
	}

	public async Task<Result<PlantsResponse>> AddAsync(PlantsRequest request, CancellationToken cancellationToken = default)
	{
		var isExistingName = await _context.Plants.AnyAsync(x => x.Name == request.Name, cancellationToken: cancellationToken);

		if (isExistingName)
			return Result.Failure<PlantsResponse>(PlantsErrors.DuplicatedPlantTitle);

		var plant = request.Adapt<Plant>();
		plant.CategoryId = request.CategoryId;

		await _context.AddAsync(plant, cancellationToken);

		await _context.SaveChangesAsync(cancellationToken);

		return Result.Success(plant.Adapt<PlantsResponse>());
	}

	public async Task<Result> UpdateAsync(int id, PlantsRequest request, CancellationToken cancellationToken = default)
	{
		var isExistingTitle = await _context.Plants.AnyAsync(x => x.Name == request.Name && x.Id != id, cancellationToken: cancellationToken);

		if (isExistingTitle)
			return Result.Failure<PlantsResponse>(PlantsErrors.DuplicatedPlantTitle);

		var currentPlant = await _context.Plants.FindAsync(id, cancellationToken);

		if (currentPlant is null)
			return Result.Failure(PlantsErrors.PlantNotFound);

		currentPlant.Name = request.Name;
		currentPlant.Price = request.Price;
		currentPlant.Description = request.Description;
		currentPlant.How_To_Plant = request.How_To_Plant;
		currentPlant.Quantity = request.Quantity;
		currentPlant.ImagePath = request.ImageUrl;
		currentPlant.Is_Avilable = request.Is_Available;
		currentPlant.CategoryId = request.CategoryId;

		await _context.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}

	public async Task<Result> IsAvailableAsync(int id, CancellationToken cancellationToken = default)
	{
		var plant = await _context.Plants.FindAsync(id, cancellationToken);

		if (plant is null)
			return Result.Failure(PlantsErrors.PlantNotFound);

		plant.Is_Avilable = !plant.Is_Avilable;

		await _context.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}
}
