using Microsoft.AspNetCore.Identity;

namespace Plant_Project.API.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() 
        { 
            Id=Guid.CreateVersion7().ToString();
			SecurityStamp = Guid.CreateVersion7().ToString();
		}    
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
		public bool IsDisabled { get; set; }
        public ICollection<Cart>? Carts { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Payment>? Payments { get; set; }
		public List<RefreshToken> RefreshTokens { get; set; } = [];
    }
}
