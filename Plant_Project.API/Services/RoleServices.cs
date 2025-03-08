using Plant_Project.API.contracts.Roles;

namespace Plant_Project.API.Services
{
    public class RoleServices(RoleManager<ApplicationRole> roleManager) : IRoleServices
    {
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

        public async Task<IEnumerable<RoleResponse>> GetAllAsync(bool? includeDisable=false)=>
            await _roleManager.Roles.Where(x=>!x.IsDefault && (!x.IsDeleted || includeDisable.HasValue  && includeDisable.Value))
            .ProjectToType<RoleResponse>()
            .ToListAsync();
   
    }
}
