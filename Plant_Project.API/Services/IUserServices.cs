using Plant_Project.API.Abstraction;
using Plant_Project.API.contracts.Posts;
using Plant_Project.API.contracts.Users;

namespace Plant_Project.API.Services
{
    public interface IUserServices
    {
        public Task<Result<UserProfileResponse>> GetProfileAsync(String UserId);
        public Task<Result> UpdateProfileAsync(string UserId,UpdateProfileRequest updateProfileRequest);
        public Task<Result> ChangePasswordAsync(String UserId, ChangePasswordRequest request);
        public Task<Result<List<PostResponse>>> UserPost(string UserId,CancellationToken cancellationToken);
        public Task<Result<List<UsersResponse>>> GetAllUser(string UserId);
        public Task<Result> DeleteUser(string UserId);
    }
}
