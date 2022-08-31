using Microsoft.AspNetCore.Identity;

namespace Final_E_Commerce.Entities
{
    public class AppUser:IdentityUser
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Fullname => $"{Firstname} {Lastname}";
        public List<Product>? Products { get; set; }
        public List<Order>? Orders { get; set; }
        public Wishlist? Wishlist { get; set; }
        //public List<BasketItem> BasketItems { get; set; }
    }
}
