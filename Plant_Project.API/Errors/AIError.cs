namespace Plant_Project.API.Errors;

public class AIError
{

	public static readonly Error NoFileUploaded = new Error("No file uploaded.","no File Uploaded");
	public static readonly Error NoDiseaseDetected = new Error("No plant disease detected.", "No plant disease detected.");
	public static readonly Error AIModelFailed = new Error("AI model failed.", "AI model failed.");
}
