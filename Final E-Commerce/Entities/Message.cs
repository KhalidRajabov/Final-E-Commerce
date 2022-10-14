namespace Final_E_Commerce.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public DateTime Date { get; set; }

        public string? AppUserId { get; set; }
        public AppUser? User { get; set; }
    }
}
