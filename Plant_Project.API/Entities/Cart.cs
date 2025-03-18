namespace Plant_Project.API.Entities
{
    public class Cart
    {
        public int Id { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public int PlantId { get; set; }

        public string PlantName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        [Precision(18, 2)]
        public decimal Price { get; set; }

        public string ImagePath { get; set; } = string.Empty;

        public Plant? Plant { get; set; }


        public decimal TotalPrice => Price * Quantity;
    }
}
