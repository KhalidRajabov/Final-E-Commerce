using Microsoft.AspNetCore.Identity;

namespace Final_E_Commerce.Entity
{
    public class AppUser:IdentityUser
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Fullname => $"{Firstname} {Lastname}";
        public List<UserProduct>? UserProducts { get; set; }
        public List<Order>? Orders { get; set; }
        //public List<BasketItem> BasketItems { get; set; }
    }
}
