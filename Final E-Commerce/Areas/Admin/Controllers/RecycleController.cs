using Final_E_Commerce.Areas.Admin.ViewModels;
using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_E_Commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
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

            List<Products>? product = _context?.Products?.Include(p => p.Category).Include(pi => pi.ProductImages)
                .Where(p => p.IsDeleted == true).Skip((page - 1) * take).Take(take).ToList();
            PaginationVM<Products> paginationVM = new PaginationVM<Products>(product, PageCount(take), page);

            return View(paginationVM);
        }

        private int PageCount(int take)
        {
            List<Products>? products = _context?.Products?.Where(p => p.IsDeleted == true).ToList();
            return (int)Math.Ceiling((decimal)products.Count() / take);
        }
        public async Task<IActionResult> ProductDetails(int? id)
        {
            if (id == null) return RedirectToAction("error", "home");
            Products? product = await _context?.Products?.Include(p=>p.Category).Include(p=>p.Brand).Include(pi => pi.ProductImages)?
            .Include(t => t.ProductTags).ThenInclude(t => t.Tags).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return RedirectToAction("error", "home");
            DetailVM detailVM = new DetailVM
            {
                Product = product,
            };
            return View(detailVM);
        }
        public async Task<IActionResult> RecoverProduct(int? id)
        {
            if (id == null) return RedirectToAction("error", "home");
            Products? product = _context?.Products?.Find(id);
            if (product == null) return RedirectToAction("error", "home");
            product.IsDeleted = false;
            product.DeletedAt = null;
            product.DeletedBy = null;
            await _context.SaveChangesAsync();
            return RedirectToAction("DeletedProducts");
        }


        public async Task<IActionResult> DeletedBrands()
        {

            List<Brand>? product = await _context?.Brands?.Where(b => b.IsDeleted).ToListAsync();
            ListBrandVM brandVM = new ListBrandVM
            {
                Brands = product
            };
            return View(brandVM);
        }

        public async Task<IActionResult> BrandDetails(int id)
        {
            BrandVM? brand = new BrandVM
            {
                Brand = await _context?.Brands?.FirstOrDefaultAsync(b => b.Id == id)
            }; 
            return View(brand);
        }
        public async Task<IActionResult> RecoverBrand(int id)
        {
            Brand? brand = await _context?.Brands?.FirstOrDefaultAsync(b => b.Id == id);
            brand.IsDeleted = false;
            brand.DeletedAt = null;
            brand.DeletedBy = null;
            await _context.SaveChangesAsync();
            return RedirectToAction("DeletedBrands");
        }


    }
}