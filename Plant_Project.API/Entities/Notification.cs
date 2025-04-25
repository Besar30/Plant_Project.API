using System.ComponentModel.DataAnnotations.Schema;

namespace Plant_Project.API.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
        public string? Message { get; set; }
        public bool IsRead { get; set; } = false; 
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        // صوره الي عامل تعليق
        public string? ImageUrl { get; set; }

        public int PostId { get; set; }

        // الربط مع البوست
        [ForeignKey("PostId")]
        public Post? Post { get; set; }
    }
}
