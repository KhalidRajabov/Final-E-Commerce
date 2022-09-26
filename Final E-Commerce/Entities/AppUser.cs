using Microsoft.AspNetCore.Identity;
 
namespace Final_E_Commerce.Entities
{
    public class AppUser:IdentityUser
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Fullname { get; set; }
        public string? ProfilePicture { get; set; }
       
        
        public List<Product>? Products { get; set; }
        public List<Order>? Orders { get; set; }
        public List<Wishlist>? Wishlist { get; set; }
        public UserDetails? UserDetails { get; set; }
        public UserProfile? UserProfile { get; set; }
        public ProductComment? ProductComment { get; set; }
        public List<Blogs>? Blogs { get; set; }
        public List<BlogComment>? BlogComment { get; set; }
        //public List<BasketItem> BasketItems { get; set; }
    }
}