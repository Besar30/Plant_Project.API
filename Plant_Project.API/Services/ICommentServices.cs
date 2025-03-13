using Plant_Project.API.contracts;
using Plant_Project.API.contracts.Comments;

namespace Plant_Project.API.Services
{
    public interface ICommentServices
    {
        Task<Result>AddComment(CommentRequest commentRequest, string userId, CancellationToken cancellationToken);
        Task<Result<List<CommentResponse>>> GetCommentsByPost(int PostId);
        Task<Result> DeleteComment(int CommentId, string UserId,CancellationToken cancellationToken);
    }
}
