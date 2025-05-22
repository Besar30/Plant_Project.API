using Plant_Project.API.contracts.Posts;

namespace Plant_Project.API.Services
{
    public interface IPostServices
    {
        Task<Result<List<PostResponse>>> GetAll(CancellationToken cancellationToken);
        Task<Result>AddPost(PostRequestDTO post,string UserId,CancellationToken cancellationToken);
        Task<Result<PostResponse>> GetById(int Id);
       Task<Result> DeletePost(int PostId,string UserId);
    }
}
