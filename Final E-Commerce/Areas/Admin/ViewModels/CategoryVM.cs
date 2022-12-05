using Final_E_Commerce.Entities;

using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Final_E_Commerce.Areas.Admin.ViewModels
{
    public class CategoryVM
    {
        public Category? Category { get; set; }
        public int ParentId { get; set; }
        [Required, MinLength(2, ErrorMessage ="Category name can not be less than 2 characters"), MaxLength(20, ErrorMessage ="Category name can not be more than 20 characters")]
        public string? Name { get; set; }
        [Required, MinLength(50, ErrorMessage = "Category description can not be less than 50 characters"), MaxLength(1000, ErrorMessage = "Category description can not be more than 1000 characters")]
        public string? Description { get; set; }
        [Required]
        public string? ImageUrl { get; set; }
        public IFormFile? Images { get; set; }
        public int ProductCount { get; set; }
    }
}
