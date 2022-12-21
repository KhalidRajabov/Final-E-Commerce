namespace Final_E_Commerce.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string? ActionBy { get; set; }
        public DateTime Time { get; set; }
        public bool Read { get; set; }
        public NotificationType NotificationType { get; set; }

        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

        public int ProductsId { get; set; }
        public Products? Products { get; set; }
    }

    public enum NotificationType
    {
        CommentOnProduct=1
    }
}
