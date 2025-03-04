namespace Plant_Project.API.Errors
{
    public class CartErrors
    {
	
		public static readonly Error CartNotFound =
			new("User.cartNotFound", "Cart is not found", StatusCodes.Status404NotFound);


		public static readonly Error ItemNotFound =
			new("User.ItemNotFound", "Item is not found", StatusCodes.Status404NotFound);

		public static readonly Error CartEmpty =
			new("User.CartEmpty", "Cart is empty", StatusCodes.Status400BadRequest);

		
	}
}
