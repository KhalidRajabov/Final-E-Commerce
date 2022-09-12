using AutoMapper;
using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_E_Commerce.Controllers
{
    public class SearchController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _usermanager;

        public SearchController(AppDbContext context, UserManager<AppUser> usermanager)
        {
            _context = context;
            _usermanager = usermanager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SearchProduct(string search)
        {
            List<Product> products = _context.Products
                .Where(p =>
                p.Name.ToLower().Contains(search.ToLower()) ||
                p.Description.ToLower().Contains(search.ToLower()) ||
                p.Body.ToLower().Contains(search.ToLower()) ||
                p.RearCamera.ToLower().Contains(search.ToLower()) ||
                p.FrontCamera.ToLower().Contains(search.ToLower()) ||
                p.Weight.ToLower().Contains(search.ToLower()) ||
                p.Display.ToLower().Contains(search.ToLower()) ||
                p.GPU.ToLower().Contains(search.ToLower()) ||
                p.OperationSystem.ToLower().Contains(search.ToLower()) ||
                p.Memory.ToLower().Contains(search.ToLower()))
                .Include(p => p.Category).Include(p => p.ProductImages)
                .OrderBy(p => p.Id)
                .Take(10).ToList();
            if (products == null)
            {
                return NotFound();
            }
            DetailVM detailVM = new DetailVM();
            detailVM.ListProducts = products;

            return PartialView("_Search", detailVM);
        }

        public IActionResult PopularProducts()
        {
            List<Product> PopularProducts = _context.Products.OrderByDescending(p => p.Views).Take(3).Include(p=>p.ProductImages).ToList();
            DetailVM detailVM = new DetailVM();
            detailVM.ListProducts= PopularProducts;
            return PartialView("_Popular", detailVM);
            
        }
    }
}
