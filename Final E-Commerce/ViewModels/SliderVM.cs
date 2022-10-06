using Microsoft.Build.Framework;

namespace Final_E_Commerce.ViewModels
{
    public class SliderVM
    {
        [Required]
        public IFormFile? Photo { get; set; }
        [Required]
        public string? Subtitle { get; set; }
        [Required]
        public string? MainTitle { get; set; }
        [Required]
        public string? Description { get; set; }
    }
}
