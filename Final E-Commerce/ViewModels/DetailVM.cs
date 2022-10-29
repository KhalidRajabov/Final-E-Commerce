using Final_E_Commerce.Entities;

namespace Final_E_Commerce.ViewModels
{
    public class DetailVM
    {
        public Products? Product { get; set; }
        public AppUser? Owner { get; set; }
        public AppUser? User { get; set; }
        public List<Products>? RelatedProducts { get; set; }
        public List<Products>? SearchProducts { get; set; }
        public List<Blogs>? Blogs { get; set; }
        public List<ProductComment>? Comments { get; set; }
        public List<AppUser>? Users { get; set; }
        public int UsersWantIt { get; set; }

    }
}
