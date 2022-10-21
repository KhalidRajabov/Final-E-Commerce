namespace Final_E_Commerce.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double? Price { get; set; }
        public double? TotalPrice { get; set; }
        public int? Count { get; set; }

        public int ProductId { get; set; }

        public int OrderId { get; set; }
        public Orders? Order { get; set; }
    }
}
