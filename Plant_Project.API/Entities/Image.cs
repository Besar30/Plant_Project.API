namespace Plant_Project.API.Entities
{
	public class Image
	{
		public Guid Id { get; set; } = Guid.NewGuid(); // Unique ID
		public string FileName { get; set; } = string.Empty;
		public string FilePath { get; set; } = string.Empty; // Example: "images/your-photo.jpg"
		public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
	}
}
