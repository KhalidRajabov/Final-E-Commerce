using Final_E_Commerce.Entities;

namespace Final_E_Commerce.ViewModels
{
    public class DetailVM
    {
        public Product? Product { get; set; }
        public AppUser? Owner { get; set; }
        public AppUser? User { get; set; }
        public List<Product>? RelatedProducts { get; set; }
        public List<Product>? SearchProducts { get; set; }
        public List<Blogs>? Blogs { get; set; }
        public List<ProductComment>? Comments { get; set; }
        public int UsersWantIt { get; set; }

    }
}
