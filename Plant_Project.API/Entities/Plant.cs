namespace Plant_Project.API.Entities
{
    public class Plant
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string Description { get; set; } = string.Empty;

        public string How_To_Plant { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public bool Is_Avilable { get; set; } = true;

        public string ImagePath { get; set; } = string.Empty;

        public int CategoryId { get; set; }


        public Category? Category { get; set; }

        public int PurchseCount { get; set; }

    }
}
