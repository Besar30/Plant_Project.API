using System.Text.Json.Serialization;

namespace Plant_Project.API.contracts.Ai
{
	public class PlantDetectionResponse
	{
		[JsonPropertyName("disease_name")]
		public string? Name { get; set; }

		[JsonPropertyName("cause")]
		public string? Cause { get; set; }

		[JsonPropertyName("treatment")]
		public string? Cure { get; set; }

		[JsonPropertyName("accuracy")]
		public string? AccuracyText { get; set; }

		[JsonPropertyName("message")]
		public string? Message { get; set; }

		[JsonPropertyName("success")]
		public bool Success { get; set; }

		[JsonIgnore]
		public float? Accuracy
		{
			get
			{
				if (string.IsNullOrWhiteSpace(AccuracyText))
					return null;

				var numericPart = AccuracyText.Replace("%", "").Trim();
				if (float.TryParse(numericPart, out float result))
					return result;

				return null;
			}
		}
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
