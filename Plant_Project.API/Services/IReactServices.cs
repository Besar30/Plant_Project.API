using Plant_Project.API.contracts.React;

namespace Plant_Project.API.Services
{
    public interface IReactServices
    {
        public Task<Result>AddReactAsync(ReactRequest reactRequest,string UserId);
        public Task<Result<List<ReactResponse>>> GetAllUserReacted(int PostId);
    }
}
