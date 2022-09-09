using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.Migrations;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_E_Commerce.Areas.Admin.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class RecycleController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public RecycleController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult DeletedProducts(int page = 1, int take = 5)
        {

            List<Product> product = _context.Products.Include(p => p.Category).Include(pi => pi.ProductImages)
                .Where(p => p.IsDeleted == true).Skip((page - 1) * take).Take(take).ToList();
            PaginationVM<Product> paginationVM = new PaginationVM<Product>(product, PageCount(take), page);

            return View(paginationVM);
        }

        private int PageCount(int take)
        {
            List<Product> products = _context.Products.Where(p => p.IsDeleted == true).ToList();
            return (int)Math.Ceiling((decimal)products.Count() / take);
        }
        public IActionResult ProductDetails(int? id)
        {
            if (id == null) return NotFound();
            Product product = _context.Products.Include(pi => pi.ProductImages)
            .Include(t => t.ProductTags).ThenInclude(t => t.Tags).FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();
            return View(product);
        }
        public async Task<IActionResult> RecoverProduct(int? id)
        {
            if (id == null) return NotFound();
            Product product = _context.Products.Find(id);
            if (product == null) return NotFound();
            product.IsDeleted = false;
            await _context.SaveChangesAsync();
            return RedirectToAction("DeletedProducts");

        }





    }
}
