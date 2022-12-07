namespace Final_E_Commerce.Entities
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public DateTime Date { get; set; }

        public string? OtherId { get; set; }
        public string? AppuserId { get; set; }
        public AppUser? AppUser { get; set; }


        public int CommunicationId { get; set; }
        public Communication? Communication { get; set; }
    }
}
