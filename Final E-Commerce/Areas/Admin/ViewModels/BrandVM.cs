using Final_E_Commerce.Entities;

namespace Final_E_Commerce.Areas.Admin.ViewModels
{
    public class BrandVM
    {
        public Brand? Brand { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
