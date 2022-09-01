using Final_E_Commerce.DTO.Bio_DTO;
using Final_E_Commerce.Entities;
using WebApi.DTO.CategoryDTO;
using WebApi.DTO.Product_DTOs;

namespace Final_E_Commerce.ViewModels
{
    public class HomeVM
    {
        public BioReturnDTO? Bio { get; set; }
        public CategoryReturnDto? Category { get; set; }
        public ProductListDto? ProductListDto { get; set; }
        public List<string>? Images { get; set; }
    }
}
