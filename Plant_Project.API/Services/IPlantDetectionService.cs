
namespace Plant_Project.API.Services;

public interface IPlantDetectionService
{
	Task<YourMappedResult> DetectPlantAsync(IFormFile file, CancellationToken cancellationToken = default);

}

