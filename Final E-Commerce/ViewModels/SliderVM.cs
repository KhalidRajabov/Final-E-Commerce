using Microsoft.Build.Framework;

namespace Final_E_Commerce.ViewModels
{
    public class SliderVM
    {
        
        public IFormFile? Photo { get; set; }
        [Required]
        public int Id { get; set; }
        [Required]
        public string? FirstTitle { get; set; }
        [Required]
        public string? Subtitle { get; set; }
        [Required]
        public string? MainTitle { get; set; }
        [Required]
        public string? Description { get; set; }

        [Required]
        public string? Link { get; set; }

        public string? ImageUrl { get; set; }
    }
}
