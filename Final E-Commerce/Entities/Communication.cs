namespace Final_E_Commerce.Entities
{
    public class Communication
    {
        public int Id { get; set; }
        public string? AppUserId { get; set; }
        public string? OtherAppUserId { get; set; }
        public List<AppUser>? Users { get; set; }
        public List<ChatMessage>? ChatMessages { get; set; }
    }
}
