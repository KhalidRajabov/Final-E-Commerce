using Final_E_Commerce.Entities;

namespace Final_E_Commerce.ViewModels
{
    public class OrderVM
    {
        public int Id { get; set; }
        public List<AppUser>? User { get; set; }
        public List<Order>? Orders { get; set; }
    }
}
