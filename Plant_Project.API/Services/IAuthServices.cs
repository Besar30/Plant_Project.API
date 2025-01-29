using Plant_Project.API.Abstraction;
using Plant_Project.API.contracts.Authentication;

namespace Plant_Project.API.Services
{
    public interface IAuthServices
    {
        Task<Result<AuthRespons>> GetTokenaync(string email, string password, CancellationToken cancellationToken = default);
        Task<Result<AuthRespons>> GetRefeshTokenaync(string Token, string RefreshToken, CancellationToken cancellationToken = default);
        Task<Result> RevokeRefeshTokenaync(string Token, string RefreshToken, CancellationToken cancellationToken = default);
    }
}
