using System.ComponentModel.DataAnnotations;

namespace Final_E_Commerce.ViewModels
{
    public class CheckoutVM
    {
        [Required, MaxLength(15, ErrorMessage ="Name can not be longer than 15"), MinLength(4, ErrorMessage = "Can not be shorter than 4")]
        public string? Firstname { get; set; }
        [Required, MaxLength(15, ErrorMessage = "Lastname can not be longer than 15"), MinLength(5, ErrorMessage = "Last name can not be shorter than 5")]
        public string? Lastname { get; set; }
        [Required, MaxLength(15, ErrorMessage = "No country has longer name than 15 letters"), MinLength(3, ErrorMessage = "No country has a name less than 3 letters")]
        public string? Country { get; set; }
        [Required]
        public string? City { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        [Required]
        public string? Company { get; set; }
        [Required]
        public string? ZipCode { get; set; }
        [Required,DataType(DataType.EmailAddress)]
        public string? Email { get; set; }


        public List<BasketVM>? Baskets { get; set; }
    }
}
