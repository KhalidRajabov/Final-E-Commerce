using Final_E_Commerce.DTO.Bio_DTO;
using Final_E_Commerce.Entities;

namespace Final_E_Commerce.ViewModels
{
    public class HeaderVM
    {
        public BioReturnDTO? Bio { get; set; }
        public List<BasketVM>? Basket { get; set; }
    }
}
