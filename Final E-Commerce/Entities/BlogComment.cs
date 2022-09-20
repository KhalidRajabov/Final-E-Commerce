namespace Final_E_Commerce.Entities
{
    public class BlogComment
    {
        public int Id { get; set; }
        public string? CommentContent { get; set; }
        public string? Author { get; set; }
        public DateTime Date { get; set; }


        public int BlogId { get; set; }
        public string? AppUserId { get; set; }
        public AppUser? User { get; set; }
        public Blogs? Blog { get; set; }
    }
}
