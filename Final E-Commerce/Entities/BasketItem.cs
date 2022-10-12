namespace Final_E_Commerce.Entities
{
    public class BasketItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public Products? Product { get; set; }
        public int BasketId { get; set; }
        public Basket? Basket { get; set; }
    }
}
