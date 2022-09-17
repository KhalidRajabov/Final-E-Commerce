using Final_E_Commerce.Entities;
using System.ComponentModel.DataAnnotations;

namespace Final_E_Commerce.ViewModels
{
	public class UserDetailsVM
	{
        [Required, MinLength(2, ErrorMessage = "Min character: 2"), MaxLength(20, ErrorMessage = "Max character: 20")]
        public string? Firstname { get; set; }
        [Required, MinLength(2, ErrorMessage = "Min character: 2"), MaxLength(20, ErrorMessage = "Max character: 20")]
        public string? Lastname { get; set; }
        [Required, MinLength(4, ErrorMessage = "Min character: 4"), MaxLength(56, ErrorMessage = "Max character: 56")]
        public string? Country { get; set; }
        [Required, MinLength(3, ErrorMessage = "Min character: 3"), MaxLength(15, ErrorMessage = "Max character: 15")]
        public string? City { get; set; }
        [Required, MinLength(5, ErrorMessage = "Min character: 4"), MaxLength(100, ErrorMessage = "Max character: 56")]
        public string? Street { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }

        public string? Company { get; set; }
        [Required]
        public string? ZipCode { get; set; }
        [Required,DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
    }
}
