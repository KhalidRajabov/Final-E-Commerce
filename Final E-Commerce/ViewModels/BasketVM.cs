namespace Final_E_Commerce.ViewModels
{
    public class BasketVM
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public int ProductCount { get; set; }
    }
}
