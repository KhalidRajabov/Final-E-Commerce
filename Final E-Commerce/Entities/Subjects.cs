namespace Final_E_Commerce.Entities
{
    public class Subjects
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public bool IsDeleted { get; set; }
        public List<BlogSubjects>? BlogSubjects { get; set; }
    }
}
