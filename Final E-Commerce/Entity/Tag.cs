namespace Final_E_Commerce.Entity
{
    public class Tag
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsDeleted { get; set; }
        public List<ProductTag>? ProductTag { get; set; }
    }
}
