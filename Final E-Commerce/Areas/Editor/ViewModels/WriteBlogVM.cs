using Final_E_Commerce.Entities;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Final_E_Commerce.Areas.Editor.ViewModels
{
    public class WriteBlogVM
    {
        [Required(ErrorMessage = "Title can not be empty"), MinLength(5, ErrorMessage ="Title characters must be 5 or longer"), MaxLength(50, ErrorMessage ="Title can not be longer than 50 characters")]
        public string? Title { get; set; }
        [Required(ErrorMessage ="Content can not be empty"), MinLength(50, ErrorMessage ="Content characters must be 50 or longer"), MaxLength(10000, ErrorMessage ="Content can not be longer than 10000 characters ")]
        public string? Content { get; set; }
        [Required(ErrorMessage = "Photo can not be empty")]
        public IFormFile? Photo { get; set; }

        [Required(ErrorMessage = "At least one tag")]
        public List<int>? SubjectId { get; set; }
        public List<Subjects>? Subjects { get; set; }
    }
}
