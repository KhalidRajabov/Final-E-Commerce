namespace Final_E_Commerce.Entities
{
    public class Wishlist
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public int ProductId { get; set; }
        public Product? Products { get; set; }
    }
}
