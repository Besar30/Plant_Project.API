namespace Plant_Project.API.Entities
{
    public class React
    {
        public int Id { get; set; } 

        public string? UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public int PostId { get; set; }
        public Post Post { get; set; } = null!;
        public DateTime LikedAt { get; set; } = DateTime.UtcNow;

    }
}
