using System.ComponentModel.DataAnnotations;

namespace Final_E_Commerce.ViewModels
{
    public class RegisterVM
    {
        [Required, StringLength(100)]
        public string? Firstname { get; set; }

        [Required, StringLength(100)]
        public string? Lastname { get; set; }


        [Required, StringLength(100)]
        public string? Username { get; set; }


        [Required, StringLength(100), DataType(DataType.EmailAddress)]
        public string? Email { get; set; }


        [Required, StringLength(16), DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required, StringLength(16), DataType(DataType.Password), Compare("Password", ErrorMessage = "Confirmation password is wrong")]
        public string? ConfirmPassword { get; set; }
    }
}
