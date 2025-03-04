namespace Plant_Project.API.Entities;

	public class Plant
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		[Precision(18, 2)]
		public decimal Price { get; set; }

		public string Description { get; set; } = string.Empty;

		public string HowToPlant { get; set; } = string.Empty;

		public int Quantity { get; set; } = 0;

		public bool IsAvailable { get; set; } = true;

		public string ImagePath { get; set; } = string.Empty;

		public int CategoryId { get; set; }

		public Category? Category { get; set; }
		public ICollection<Cart>? Carts { get; set; }

		public int PurchaseCount { get; set; } =0;

	}

 