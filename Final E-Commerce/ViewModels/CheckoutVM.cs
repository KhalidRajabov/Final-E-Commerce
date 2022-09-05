using System.ComponentModel.DataAnnotations;

namespace Final_E_Commerce.ViewModels
{
    public class CheckoutVM
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Company { get; set; }
        public string? ZipCode { get; set; }
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }


        //public List<BasketVM>? Baskets { get; set; }
    }
}
