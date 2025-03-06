using Microsoft.AspNetCore.Authorization;

namespace Plant_Project.API.Authentication.Filters
{
    public class HasPermissionAttribute(string Permission) :AuthorizeAttribute(Permission)
    {

    }
}
