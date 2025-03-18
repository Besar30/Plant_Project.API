

namespace Plant_Project.API.Entities
{
    public sealed class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string ImagePath {  get; set; }=string.Empty;
        public List<RefreshToken> RefreshTokens { get; set; } = [];


        public List<Post> Posts { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();



        public ICollection<Cart>? Carts { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Payment>? Payments { get; set; }
    }
}
