namespace Plant_Project.API.Entities
{
    public class Comment {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //user
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;
        //post
        public int PostId { get; set; }
        public Post Post { get; set; } = null!;
    }
}
