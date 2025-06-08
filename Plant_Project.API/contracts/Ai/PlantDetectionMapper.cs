namespace Plant_Project.API.contracts.Ai;


	public class PlantDetectionMapper
	{
		public YourMappedResult Map(PlantDetectionResponse response)
		{
			if (response == null || response.Prediction == null)
				throw new ArgumentNullException(nameof(response), "Plant detection response cannot be null or empty.");

			var prediction = response.Prediction;

			if (string.IsNullOrEmpty(prediction.Name) || prediction.Name.Contains("Background_without_leaves"))
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

			if (string.IsNullOrEmpty(prediction.Cause))
			{
				return new YourMappedResult
				{
					PlantName = prediction.Name ?? "Unknown Plant",
					HasDisease = false,
					Disease = "No disease detected.",
					Solution = "No solution available.",
					Accuracy = prediction.Accuracy ?? 0f
				};
			}

			return new YourMappedResult
			{
				PlantName = prediction.Name ?? "Unknown Plant",
				HasDisease = !string.IsNullOrEmpty(prediction.Cause),  // If there's no cause, we assume no disease
				Disease = prediction.Cause ?? "No disease detected.",
				Solution = prediction.Cure ?? "No solution available.",
				Accuracy = prediction.Accuracy ?? 0f

			};
		}
	}


