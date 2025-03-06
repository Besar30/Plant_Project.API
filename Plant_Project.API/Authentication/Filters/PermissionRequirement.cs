using Microsoft.AspNetCore.Authorization;

namespace Plant_Project.API.Authentication.Filters
{
    public class PermissionRequirement(string Permission):IAuthorizationRequirement
    {
        public string Permission { get; } = Permission;

    }
}
