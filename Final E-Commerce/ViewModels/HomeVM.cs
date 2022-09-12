using Final_E_Commerce.DTO.Bio_DTO;
using Final_E_Commerce.Entities;
using WebApi.DTO.CategoryDTO;
using WebApi.DTO.Product_DTOs;

namespace Final_E_Commerce.ViewModels
{
    public class HomeVM
    {
        public Bio? Bio { get; set; }
        public Category? Category { get; set; }
        public List<Product>? Products { get; set; }
        public List<Product>? BestSellerProducts { get; set; }
    }
}
