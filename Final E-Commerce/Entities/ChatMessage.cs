namespace Final_E_Commerce.Entities
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public DateTime Date { get; set; }

        public string? ReceiverId { get; set; }
        public string? AppUserId { get; set; }
        public AppUser? User { get; set; }
    }
}
