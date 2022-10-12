namespace Final_E_Commerce.Entities
{
    public class ProductTag
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int TagId { get; set; }

        public Products? Product { get; set; }
        public Tag? Tags { get; set; }
    }
}
