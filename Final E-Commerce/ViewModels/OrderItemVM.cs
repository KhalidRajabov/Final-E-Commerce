using Final_E_Commerce.Entities;

namespace Final_E_Commerce.ViewModels
{
    public class OrderItemVM
    {
        public int Id { get; set; }
        public AppUser? User { get; set; }
        public Order? Order { get; set; }
        public List<OrderItem>? OrderItems { get; set; }
    }
}
