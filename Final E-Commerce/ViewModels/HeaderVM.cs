using Final_E_Commerce.DTO.Bio_DTO;
using Final_E_Commerce.Entities;

namespace Final_E_Commerce.ViewModels
{
    public class HeaderVM
    {
        public Bio? Bio { get; set; }
        public List<BasketVM>? BasketProducts { get; set; }
    }
}
