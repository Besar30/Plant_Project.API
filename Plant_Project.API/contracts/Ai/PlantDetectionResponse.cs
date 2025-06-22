namespace Plant_Project.API.contracts.Ai
{
	public class PlantDetectionResponse
	{
		public string? ImageUrl { get; set; }   
		public Prediction? Prediction { get; set; } 
	}

	public class Prediction
	{
		public string? Name { get; set; }
		public string? Cause { get; set; }
		public string? Cure { get; set; }
		public float? Accuracy { get; set; }
	}

	public record YourMappedResult
	{
		public string? PlantName { get; set; }
		public bool HasDisease { get; set; }  
		public string? Disease { get; set; }
		public string? Solution { get; set; }
		public float? Accuracy { get; set; }
	}
	public class PlantDetectionResponseDto
	{
		public bool Success { get; set; }
		public string? Message { get; set; }
		public YourMappedResult? Data { get; set; }
	}

}
