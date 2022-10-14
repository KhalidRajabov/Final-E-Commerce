using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Final_E_Commerce.ViewModels
{
    public class ForgotPasswordVM
    {
        [Required, DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
    }
}
