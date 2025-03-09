namespace Plant_Project.API.Errors;

public static class CategoryError
{
	
	public static readonly Error CategoryNotFound =
			new("User.CategoryNotFound", "Category is not found", StatusCodes.Status404NotFound);

	public static readonly Error CategoryDublicated =
	   new ("Category Dublicated", "Category already Exist", StatusCodes.Status409Conflict);


}
