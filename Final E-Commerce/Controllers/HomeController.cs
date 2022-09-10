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
using Microsoft.AspNetCore.Identity;

namespace Final_E_Commerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _usermanager;
        public HomeController(AppDbContext context, IMapper mapper, UserManager<AppUser> usermanager)
        {
            _context = context;
            _mapper = mapper;
            _usermanager = usermanager;
        }
        public async Task<IActionResult> Index()
        {

            HomeVM homeVM = new HomeVM();
            homeVM.Bio = _context.Bios.FirstOrDefault();
            homeVM.Category = _context.Categories.FirstOrDefault(c=>c.Id==1);
            homeVM.Products = _context.Products.OrderByDescending(p=>p.Views).Take(3).Include(p=>p.ProductImages).ToList();
            return View(homeVM);
        }
        public async Task<IActionResult> Detail(int? id)
        {
            Product product = _context.Products
                .Include(p => p.ProductImages)
                .Include(c => c.Category)
                .Include(p => p.Brand)
                .Include(p => p.ProductTags)
                .ThenInclude(t => t.Tags)
                .FirstOrDefault(p => p.Id == id);
            ViewBag.ExistWishlist = false;
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
                bool IsExist = _context.Wishlists.Where(w => w.AppUserId == user.Id && w.ProductId == id).Any();
                if (IsExist)
                {
                    ViewBag.ExistWishlist = true;
                }
            }
            product.Views++;
            await _context.SaveChangesAsync();
            DetailVM detailVM = new DetailVM();
            detailVM.Product = product;
            //detailVM.RelatedProducts= _context.Products.Where(c => c.CategoryId == product.CategoryId).ToList();

            return View(detailVM);
        }
        public IActionResult Error()
        {
            return View();
        }
        public IActionResult SearchProduct(string search)
        {
            List<Product> products = _context.Products
                .Where(p => 
                p.Name.ToLower().Contains(search.ToLower()) || 
                p.Description.ToLower().Contains(search.ToLower()) ||
                p.Body.ToLower().Contains(search.ToLower())||
                p.RearCamera.ToLower().Contains(search.ToLower())||
                p.FrontCamera.ToLower().Contains(search.ToLower())||
                p.Weight.ToLower().Contains(search.ToLower())||
                p.Display.ToLower().Contains(search.ToLower())||
                p.GPU.ToLower().Contains(search.ToLower())||
                p.OperationSystem.ToLower().Contains(search.ToLower())||
                p.Memory.ToLower().Contains(search.ToLower()))
                .Include(p => p.Category).Include(p=>p.ProductImages)
                .OrderBy(p => p.Id)
                .Take(10).ToList();
            if (products==null)
            {
                return NotFound();
            }
            DetailVM detailVM = new DetailVM();
            detailVM.ListProducts = products;

            return PartialView("_SearchPartial", detailVM);
        }
    }
}