

using Plant_Project.API.contracts.Plants;

namespace Plant_Project.API.Services
{
    public interface IplantServices
    {
        public Task<Result<List<PlantsResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
        public Task<Result> AddPlantAsync(PlantsRequest request, CancellationToken cancellationToken=default);
    }
}
