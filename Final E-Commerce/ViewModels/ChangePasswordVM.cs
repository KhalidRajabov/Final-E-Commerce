using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Final_E_Commerce.ViewModels
{
	public class ChangePasswordVM
	{
		[Required, MinLength(8), MaxLength(16), DataType(DataType.Password),]
		public string? NewPassword { get; set; }
        [Required,MinLength(8), MaxLength(16), DataType(DataType.Password), Compare("Password", ErrorMessage = "Confirmation password is wrong")]
        public string? ConfirmPassword { get; set; }
    }
}
