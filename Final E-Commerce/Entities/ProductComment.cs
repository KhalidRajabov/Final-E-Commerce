namespace Final_E_Commerce.Entities
{
    public class ProductComment
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public DateTime Date { get; set; }
        public string? Author { get; set; }
        public bool IsDeleted { get; set; }


        public string? AppUserId { get; set; }
        public AppUser? User { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
