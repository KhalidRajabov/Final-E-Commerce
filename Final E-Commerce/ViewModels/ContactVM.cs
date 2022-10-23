using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Final_E_Commerce.ViewModels
{
    public class ContactVM
    {
        [Required, MinLength(3), MaxLength(15)]
        public string? Firstname { get; set; }
        [Required, MinLength(3), MaxLength(15)] 
        public string? Lastname { get; set; }
        [Required, MinLength(6), MaxLength(30)]
        public string? Subject { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required, MinLength(15), MaxLength(300)]
        public string? Message { get; set; }
    }
}
