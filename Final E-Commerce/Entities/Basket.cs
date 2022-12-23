namespace Final_E_Commerce.Entities
{
    public class Basket
    {
        public int Id { get; set; }
        public string? AppUserId { get; set; }
        public AppUser? User { get; set; }
        public List<BasketItem>? BasketItems { get; set; }
    }
}
