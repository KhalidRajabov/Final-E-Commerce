namespace Final_E_Commerce.Entity
{
    public class ProductTag
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int TagId { get; set; }

        public Product? Product { get; set; }
        public Tag? Tags { get; set; }
    }
}
