using Final_E_Commerce.Entities;

namespace Final_E_Commerce.ViewModels
{
    public class CommentsVM
    {
        public AppUser? User { get; set; }
        public List<BlogComment>? BlogComments { get; set; }
        public BlogComment? Comment { get; set; }
        public List<ProductComment>? ProductComments { get; set; }
        public ProductComment? ProductComment { get; set; }
        public int RightCounter { get; set; }
        public string? UserId { get; set; }
        public string? AppUserId { get; set; }
    }
}
