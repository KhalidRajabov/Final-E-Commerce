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
            
            var images = await _context.Products.Include(p => p.ProductImages)
                .SelectMany(p => p.ProductImages, (p, img) => new { img.ImageUrl }).ToListAsync();
            
            HomeVM homevm = new HomeVM();
            Bio bio = await _context.Bios.FirstOrDefaultAsync();
            Category category = await _context.Categories.FirstOrDefaultAsync(i => i.Id == 1);
            var query = _context.Products.Include(p => p.ProductImages).AsQueryable();
            List<ProductReturnDto> productReturnDtos = _mapper.Map<List<ProductReturnDto>>(query.ToList());
            
            homevm.ProductListDto = _mapper.Map<ProductListDto>(productReturnDtos);
            homevm.Category = _mapper.Map<CategoryReturnDto>(category);
            homevm.Bio = _mapper.Map<BioReturnDTO>(bio);
            homevm.Images = new List<string>();
            foreach (var item in images)
            {
                homevm.Images.Add(item.ImageUrl);
            }

            return View(homevm);
        }
    }
}