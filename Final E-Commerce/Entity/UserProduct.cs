namespace Final_E_Commerce.Entity
{
    public class UserProduct
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int ProductId { get; set; }

        public AppUser? User { get; set; }
        public Product? Product { get; set; }
    }
}
