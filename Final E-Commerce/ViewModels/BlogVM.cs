using Final_E_Commerce.Entities;
using System.ComponentModel.DataAnnotations;

namespace Final_E_Commerce.ViewModels
{
    public class BlogVM
    {
        public Blogs? Blog { get; set; }
        [Required, MinLength(3, ErrorMessage ="Name can not be shorter than 3 letters"), MaxLength(35, ErrorMessage ="Name can not be longer than 35")]
        public string? Author { get; set; }
        [Required, MinLength(3, ErrorMessage = "Comment length can not be shorter than 3 letters"), MaxLength(500, ErrorMessage = "Comment length can not be longer than 500")]
        public string? CommentContent { get; set; }
    }
}
