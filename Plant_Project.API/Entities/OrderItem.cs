namespace Plant_Project.API.Entities
{
    public class OrderItem
    {
        public int Id { get; set; } // Primary Key

		public string? UserId { get; set; } // Foreign Key from AspNetUsers
		public ApplicationUser? User { get; set; }

		public int OrderId { get; set; } // Foreign Key to Order
        public Order? Order { get; set; }

        public int PlantId { get; set; } // Foreign Key to Plant
        public Plant? Plant { get; set; }

        public string PlantName { get; set; } = string.Empty; // Storing name for historical reference

        public int Quantity { get; set; } // Number of plants ordered

        [Precision(18, 2)]
        public decimal Price { get; set; } // Price per unit

        public decimal TotalPrice => Price * Quantity; // Total price for this plant

    }
}
