namespace Final_E_Commerce.Entities
{
    public class BlogSubject
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        public int SubjectsId { get; set; }

        public Subjects? Subjects { get; set; }
        public Blogs? Blog { get; set; }
    }
}
