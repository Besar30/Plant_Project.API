namespace Plant_Project.API.Errors;

public class OrderErrors
{
	public static readonly Error OrderNotFound =
		new("Order.OrderNotFound", "Order is not found");
	public static readonly Error OrderIdNotFound =
		new("OrderID.OrderIDNotFound", "Order ID is not found");
	public static readonly Error ItemNotFound =
		new("Item.ItemNotFound", "Item is not found");
}
