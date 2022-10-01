using Final_E_Commerce.Entities;

namespace Final_E_Commerce.ViewModels
{
    public class CommentsVM
    {
        public List<BlogComment>? BlogComments { get; set; }
        public BlogComment? Comment { get; set; }
        public int RightCounter { get; set; }
        public string? UserId { get; set; }
    }
}
