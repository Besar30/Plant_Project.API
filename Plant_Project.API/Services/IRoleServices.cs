using Plant_Project.API.contracts.Roles;

namespace Plant_Project.API.Services
{
    public interface IRoleServices
    {
        public Task<IEnumerable<RoleResponse>> GetAllAsync(bool? includeDisable = false); 
    }
}
