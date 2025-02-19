namespace Plant_Project.API.Services;

public interface IPlantServices
{
	Task<IEnumerable<PlantsResponse>> GetAllAsync(CancellationToken cancellationToken = default);
	Task<Result<PlantsResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
	Task<Result<PlantsResponse>> AddAsync(PlantsRequest request, CancellationToken cancellationToken = default);
	Task<Result> UpdateAsync(int id, PlantsRequest request, CancellationToken cancellationToken = default);

	Task<Result> IsAvailableAsync(int id, CancellationToken cancellationToken = default);
}
