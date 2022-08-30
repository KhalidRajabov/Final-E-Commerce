namespace Final_E_Commerce.Entity
{
    public class Basket
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public AppUser? User { get; set; }
        public List<BasketItem>? BasketItems { get; set; }
    }
}
