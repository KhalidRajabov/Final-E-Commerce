using AutoMapper;
using Final_E_Commerce.DAL;
using Final_E_Commerce.DTO.Bio_DTO;
using Final_E_Commerce.Entities;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.DTO.CategoryDTO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using WebApi.DTO.Product_DTOs;

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

            HomeVM homeVM = new HomeVM();
            homeVM.Bio = _context.Bios.FirstOrDefault();
            homeVM.Category = _context.Categories.FirstOrDefault(c=>c.Id==1);
            homeVM.Products = _context.Products.OrderBy(p=>p.Id).Take(3).Include(p=>p.ProductImages).ToList();
            return View(homeVM);
        }
        public async Task<IActionResult> Detail(int? id)
        {
            Product product = _context.Products.Include(p=>p.ProductImages).FirstOrDefault(p=>p.Id==id);
            
            DetailVM detailVM = new DetailVM();
            detailVM.Product = product;
            detailVM.RelatedProducts= _context.Products.Where(c => c.CategoryId == product.CategoryId).ToList();

            return View(detailVM);
        }
    }
}