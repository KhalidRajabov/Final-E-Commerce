namespace Final_E_Commerce.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Subject { get; set; }
        public string? Content { get; set; }
        public string? Email { get; set; }
        public DateTime Date { get; set; }

        public string? Answer { get; set; }
        public bool IsAnswered { get; set; }
        public DateTime AnsweredDate { get; set; }
        public string? AnsweredBy { get; set; }

        public string? AppUserId { get; set; }
        public AppUser? User { get; set; }
    }
}