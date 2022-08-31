using AutoMapper;
using Final_E_Commerce.DAL;
using Final_E_Commerce.DTO.Bio_DTO;
using Final_E_Commerce.Entities;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_E_Commerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public HomeController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            HomeVM homevm = new HomeVM();
            Bio bio = await _context.Bios.FirstOrDefaultAsync();
            BioReturnDTO biodto = new BioReturnDTO();
            homevm.Bio = _mapper.Map<BioReturnDTO>(bio);
            return View(homevm);
        }
    }
}