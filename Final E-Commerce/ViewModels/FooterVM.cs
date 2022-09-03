using Final_E_Commerce.DTO.Bio_DTO;
using Final_E_Commerce.Entities;

namespace Final_E_Commerce.ViewModels
{
    public class FooterVM
    {
        public Bio? Bio { get; set; }
        public List<Category>? Categories { get; set; }
    }
}
