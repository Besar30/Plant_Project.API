using Plant_Project.API.contracts.Roles;

namespace Plant_Project.API.Services
{
    public class RoleServices(RoleManager<ApplicationRole> roleManager,ApplicationDbContext context) : IRoleServices
    {
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<RoleResponse>> GetAllAsync(bool? includeDisable=false,CancellationToken cancellationToken=default)=>
            await _roleManager.Roles.Where(x=>!x.IsDefault && (!x.IsDeleted || includeDisable.HasValue  && includeDisable.Value))
            .ProjectToType<RoleResponse>()
            .ToListAsync(cancellationToken);


        public async Task<Result<RoleDetailResponse>> GetAsync(string id)
        {
            if (await _roleManager.FindByIdAsync(id) is not { } role)
                return Result.Failure<RoleDetailResponse>(RoleErrors.RoleNotFound);

            var permissions = await _roleManager.GetClaimsAsync(role);
            var response = new RoleDetailResponse(role.Id, role.Name!, role.IsDeleted, permissions.Select(x=>x.Value));
            return Result.Success(response);
        }

        public async Task<Result<RoleDetailResponse>> AddAsync(RoleRequest request)
        {
            var roleIsExists = await _roleManager.RoleExistsAsync(request.Name);
            if (roleIsExists)
                return Result.Failure<RoleDetailResponse>(RoleErrors.DuplicatedRole);
            var allowedPermissions=Permissions.GetAllPermissions();
            if (request.Permissions.Except(allowedPermissions).Any())
            {
                return Result.Failure<RoleDetailResponse>(RoleErrors.InvalidPermissions);
            }
            var role = new ApplicationRole
            {
                Name = request.Name,
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
             var result= await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                var permissions = request.Permissions
                    .Select(x=> new IdentityRoleClaim<string>
                    {
                        ClaimType=Permissions.Type,
                        ClaimValue=x,
                        RoleId=role.Id
                    });
                await _context.AddRangeAsync(permissions);
                await _context.SaveChangesAsync();
                var respons=new RoleDetailResponse(role.Id,role.Name,role.IsDeleted,request.Permissions);
                return Result.Success(respons);
            }
            var error = result.Errors.First();
            return Result.Failure<RoleDetailResponse>(new Error(error.Code,error.Description));
        }

        public async Task<Result> UpdateAsync(string Id,RoleRequest request)
        {
            var roleIsExists = await _roleManager.Roles.AnyAsync(x => x.Name == request.Name && x.Id != Id);
            if (roleIsExists)
                return Result.Failure<RoleDetailResponse>(RoleErrors.DuplicatedRole);

            if (await _roleManager.FindByIdAsync(Id) is not { } role)
                return Result.Failure(RoleErrors.RoleNotFound);

            var allowedPermissions = Permissions.GetAllPermissions();
            if (request.Permissions.Except(allowedPermissions).Any())
            {
                return Result.Failure(RoleErrors.InvalidPermissions);
            }

            role.Name = request.Name;
            

            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                var currentPermissions = await _context.RoleClaims
                    .Where(x => x.RoleId == Id && x.ClaimType == Permissions.Type)
                    .Select(x => x.ClaimValue)
                    .ToListAsync();
                var newPermissions = request.Permissions.Except(currentPermissions).Select(x => new IdentityRoleClaim<string>
                {
                    ClaimType = Permissions.Type,
                    ClaimValue = x,
                    RoleId = Id
                });
                var removedPermissions=currentPermissions.Except(request.Permissions);
                await _context.RoleClaims
                    .Where(x => x.RoleId == Id && removedPermissions.Contains(x.ClaimValue))
                     .ExecuteDeleteAsync();
                await _context.AddRangeAsync(newPermissions);
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description));
        }

        public async Task<Result> ToggleStatusAsync(string Id)
        {
            if(await _roleManager.FindByIdAsync(Id) is not { } role)
                return Result.Failure<RoleDetailResponse>(RoleErrors.RoleNotFound);

            role.IsDeleted=!role.IsDeleted;
            await _roleManager.UpdateAsync(role);
            return Result.Success();
        }
    }
}
