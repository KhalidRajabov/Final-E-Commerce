namespace Final_E_Commerce.Entities
{
    public class UserProductRatings
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public int ProductId { get; set; }
        public int Rating { get; set; }

        public AppUser? User { get; set; }
        public Product? Product { get; set; }

    }
}
