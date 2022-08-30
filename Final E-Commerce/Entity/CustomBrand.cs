using System.ComponentModel.DataAnnotations.Schema;

namespace Final_E_Commerce.Entity
{
    public class CustomBrand:BaseEntity
    {

        public string? ImageUrl { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }

        public List<Product>? Products { get; set; }
    }
}
