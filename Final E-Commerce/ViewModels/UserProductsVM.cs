using Final_E_Commerce.Entities;

namespace Final_E_Commerce.ViewModels
{
    public class UserProductsVM
    {
        public List<Products>? Products { get; set; }
        public AppUser? User { get; set; }
    }
}
