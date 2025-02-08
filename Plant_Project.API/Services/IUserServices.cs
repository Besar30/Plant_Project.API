using Plant_Project.API.Abstraction;
using Plant_Project.API.contracts.Users;

namespace Plant_Project.API.Services
{
    public interface IUserServices
    {
        public Task<Result<UserProfileResponse>> GetProfileAsync(String UserId);
    }
}
