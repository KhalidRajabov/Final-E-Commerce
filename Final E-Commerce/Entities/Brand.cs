using System.ComponentModel.DataAnnotations.Schema;

namespace Final_E_Commerce.Entities
{
    public class Brand:BaseEntity
    {
        
        public string? ImageUrl { get; set; }
        public bool Popular { get; set; }
        [NotMapped]                      
        public IFormFile? Photo { get; set; }




        public List<Product>? Products { get; set; }
    }
}
