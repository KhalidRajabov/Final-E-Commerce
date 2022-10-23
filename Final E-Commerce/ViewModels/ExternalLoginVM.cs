using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Final_E_Commerce.ViewModels
{
    public class ExternalLoginVM
    {
        [Required]
        public string? Username { get; set; }
    }
}
