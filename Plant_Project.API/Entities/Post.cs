namespace Plant_Project.API.Entities
{
    public class Post
    {

        public int Id { get; set; }
        public string Content { get; set; }=string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        //user
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        //comment
        public List<Comment> Comments { get; set; } = new();


    }
}
