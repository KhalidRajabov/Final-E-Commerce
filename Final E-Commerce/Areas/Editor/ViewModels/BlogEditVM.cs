using Final_E_Commerce.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_E_Commerce.Areas.Editor.ViewModels
{
    public class BlogEditVM
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        
        public IFormFile? Photo { get; set; }
        public List<int>? SubjectId { get; set; }
        public List<Subjects>? Subjects { get; set; }
    }
}
