
namespace Plant_Project.API.Services;

public interface IPlantDetectionService
{
	Task<Result<YourMappedResult>> DetectPlantAsync(IFormFile file, CancellationToken cancellationToken = default);

}

