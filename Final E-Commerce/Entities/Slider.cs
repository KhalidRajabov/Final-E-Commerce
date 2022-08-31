using System.ComponentModel.DataAnnotations.Schema;

namespace Final_E_Commerce.Entities
{
    public class Slider
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }
        public string? Subtitle { get; set; }
        public string? MainTitle { get; set; }
        public string? Description { get; set; }
    }
}
