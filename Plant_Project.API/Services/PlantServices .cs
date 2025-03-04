

using File = System.IO.File;

namespace Plant_Project.API.Services;

public class PlantServices(ApplicationDbContext context,ILogger<PlantServices> logger) : IPlantServices
{
	private readonly ApplicationDbContext _context = context;
	private readonly ILogger<PlantServices> _logger = logger;
	public async Task<IEnumerable<PlantsResponse>> GetAllAsync(CancellationToken cancellationToken = default) =>
		await _context.Plants
			.AsNoTracking()
			.ProjectToType<PlantsResponse>()
			.ToListAsync(cancellationToken);

	public async Task<Result<PlantsResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
	{
		var plant = await _context.Plants.FindAsync(id, cancellationToken);

		return plant is not null
			? Result.Success(plant.Adapt<PlantsResponse>())
			: Result.Failure<PlantsResponse>(PlantsErrors.PlantNotFound);
	}

	public async Task<Result<PlantsResponse>> AddAsync(PlantsRequest request, CancellationToken cancellationToken = default)
	{
		var isExistingName = await _context.Plants.AnyAsync(x => x.Name == request.Name, cancellationToken);

		if (isExistingName)
			return Result.Failure<PlantsResponse>(PlantsErrors.DuplicatedPlantTitle);

		string imagePath = await SaveImageAsync(request.ImageUrl);

		var plant = request.Adapt<Plant>();
		plant.CategoryId = request.CategoryId;
		plant.ImagePath = imagePath;

		await _context.AddAsync(plant, cancellationToken);
		await _context.SaveChangesAsync(cancellationToken);

		return Result.Success(plant.Adapt<PlantsResponse>());
	}

	public async Task<Result> UpdateAsync(int id, PlantsRequest request, CancellationToken cancellationToken = default)
	{
		var isExistingName = await _context.Plants.AnyAsync(x => x.Name == request.Name && x.Id != id, cancellationToken);

		if (isExistingName)
			return Result.Failure<PlantsResponse>(PlantsErrors.DuplicatedPlantTitle);

		var currentPlant = await _context.Plants.FindAsync(id, cancellationToken);

		if (currentPlant is null)
			return Result.Failure(PlantsErrors.PlantNotFound);

		if (!string.IsNullOrEmpty(currentPlant.ImagePath))
		{
			DeleteImage(currentPlant.ImagePath);
		}

		string imagePath = await SaveImageAsync(request.ImageUrl);

		currentPlant.Name = request.Name;
		currentPlant.Price = request.Price;
		currentPlant.Description = request.Description;
		currentPlant.HowToPlant = request.How_To_Plant;
		currentPlant.Quantity = request.Quantity;
		currentPlant.ImagePath = imagePath;
		currentPlant.IsAvailable = request.Is_Available;
		currentPlant.CategoryId = request.CategoryId;

		await _context.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}

	public async Task<Result> IsAvailableAsync(int id, CancellationToken cancellationToken = default)
	{
		var plant = await _context.Plants.FindAsync(id, cancellationToken);

		if (plant is null)
			return Result.Failure(PlantsErrors.PlantNotFound);

		plant.IsAvailable = !plant.IsAvailable;

		await _context.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}

	private async Task<string> SaveImageAsync(string base64Image)
	{
		if (string.IsNullOrEmpty(base64Image) || !IsBase64String(base64Image))
			return Path.Combine("Images", "R.jpg"); ;

		string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");

		if (!Directory.Exists(uploadsFolder))
		{
			Directory.CreateDirectory(uploadsFolder);
		}

		string fileName = $"{Guid.CreateVersion7()}.jpg";
		string filePath = Path.Combine(uploadsFolder, fileName);

		try
		{
			byte[] imageBytes = Convert.FromBase64String(base64Image);
			await File.WriteAllBytesAsync(filePath, imageBytes);
		}
		catch (FormatException)
		{
			 _logger.LogError("wrong formate");
		}

		return Path.Combine("Images", fileName);
	}

	private void DeleteImage(string relativeImagePath)
	{
		string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativeImagePath);

		if (File.Exists(filePath))
		{
			File.Delete(filePath);
		}
	}

	private static bool IsBase64String(string base64)
	{
		base64 = base64.Trim();
		return (base64.Length % 4 == 0) && Regex.IsMatch(base64, @"^[a-zA-Z0-9\+/]*={0,2}$");
	}
}
