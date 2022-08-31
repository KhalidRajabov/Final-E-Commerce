using AutoMapper;
using Final_E_Commerce.DAL;
using Final_E_Commerce.DTO.Bio_DTO;
using Final_E_Commerce.Entities;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_E_Commerce.ViewComponents
{
    public class HeaderViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public HeaderViewComponent(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            HeaderVM hdVM = new HeaderVM();
            Bio bio = await _context.Bios.FirstOrDefaultAsync();
            BioReturnDTO biodto = new BioReturnDTO();
            hdVM.Bio = _mapper.Map<BioReturnDTO>(bio);
            return View(await Task.FromResult(hdVM));
        }
    }
}
