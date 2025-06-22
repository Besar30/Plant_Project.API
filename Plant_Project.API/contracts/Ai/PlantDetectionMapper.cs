namespace Plant_Project.API.contracts.Ai
{
	public class PlantDetectionMapper
	{
		public YourMappedResult Map(PlantDetectionResponse response)
		{
			if (response == null || response.Prediction == null)
				throw new ArgumentNullException(nameof(response), "Plant detection response cannot be null or empty.");

			var prediction = response.Prediction;

			// If no name returned from AI
			if (string.IsNullOrEmpty(prediction.Name))
			{
				return new YourMappedResult
				{
					PlantName = "Unknown Plant",
					HasDisease = false,
					Disease = "No plant detected in the image.",
					Solution = "Try using a clearer image with a visible plant.",
					Accuracy = prediction.Accuracy ?? 0f
				};
			}

			// If no cause => No disease
			if (string.IsNullOrEmpty(prediction.Cause))
			{
				return new YourMappedResult
				{
					PlantName = prediction.Name,
					HasDisease = false,
					Disease = "No disease detected.",
					Solution = "No solution available.",
					Accuracy = prediction.Accuracy ?? 0f
				};
			}

			// Disease detected
			return new YourMappedResult
			{
				PlantName = prediction.Name,
				HasDisease = true,
				Disease = prediction.Cause,
				Solution = prediction.Cure ?? "No solution available.",
				Accuracy = prediction.Accuracy ?? 0f
			};
		}
	}
}
