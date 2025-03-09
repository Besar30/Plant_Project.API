using Plant_Project.API.contracts.Roles;

namespace Plant_Project.API.Services
{
    public interface IRoleServices
    {

        Task<IEnumerable<RoleResponse>> GetAllAsync(bool? includeDisabled = false, CancellationToken cancellationToken = default);
        Task<Result<RoleDetailResponse>> GetAsync(string id);
        Task<Result<RoleDetailResponse>> AddAsync(RoleRequest request);
        Task<Result> UpdateAsync(string Id, RoleRequest request);
        Task<Result> ToggleStatusAsync(string Id);

    }
}
