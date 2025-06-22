using System.Text.Json.Serialization;

namespace Plant_Project.API.contracts.Ai
{
	public class PlantDetectionResponse
	{
		[JsonPropertyName("image_url")]
		public string? ImageUrl { get; set; }

		[JsonPropertyName("prediction")]
		public Prediction? Prediction { get; set; }
	}

	public class Prediction
	{
		[JsonPropertyName("الاسم")]
		public string? Name { get; set; }

		[JsonPropertyName("السبب")]
		public string? Cause { get; set; }

		[JsonPropertyName("العلاج")]
		public string? Cure { get; set; }

		[JsonPropertyName("الدقه")]
		public string? AccuracyText { get; set; }

		[JsonIgnore]
		public float? Accuracy
		{
			get
			{
				if (string.IsNullOrWhiteSpace(AccuracyText))
					return null;

				var numericPart = AccuracyText.Replace("%", "").Trim();
				if (float.TryParse(numericPart, out float result))
				{
					return result;
				}
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
