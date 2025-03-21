

using Plant_Project.API.contracts.Common;
using Plant_Project.API.contracts.Plants;

namespace Plant_Project.API.Services
{
    public interface IplantServices
    {
        public Task<Result<PaginatedList<PlantsResponse>>> GetAllAsync(RequestFilters filters,CancellationToken cancellationToken = default);
        public Task<Result> AddPlantAsync(PlantsRequest request, CancellationToken cancellationToken=default);
        public Task<Result> UpdatePlantAsync(int Id,PlantsRequest request, CancellationToken cancellationToken=default);
        public Task<Result<PlantsResponse>> GetByIdAsync(int Id, CancellationToken cancellationToken=default);  
        public Task<Result<PlantsResponse>> GetByNameAsync(string Name, CancellationToken cancellationToken=default);
        public Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);

    }
}
