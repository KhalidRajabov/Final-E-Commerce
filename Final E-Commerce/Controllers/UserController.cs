using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.Extensions;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Final_E_Commerce.Controllers
{
    [Authorize]
    public class UserController:Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _usermanager;
        private readonly IWebHostEnvironment _env;

        public UserController(AppDbContext context,
        UserManager<AppUser> userManager,
        IWebHostEnvironment env)
        {
            _context = context;
            _usermanager = userManager;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            UserVM userVM = new UserVM();
            
            userVM.User = user;
            userVM.UserProfile = await _context.UserProfiles.FirstOrDefaultAsync(up => up.AppUserId == user.Id);
            userVM.UserDetails = await _context.UserDetails.FirstOrDefaultAsync(up => up.AppUserId == user.Id);
            return View(userVM);
        }
        public async Task<IActionResult> Orders()
        {
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            List<Order> order = _context.Orders.Where(o => o.AppUserId == user.Id).OrderByDescending(o => o.Id).ToList();
            OrderVM orderVM = new OrderVM();
            orderVM.Orders = order;
            return View(orderVM);
        }

        public async Task<IActionResult> Detail(int id)
        {

            Order order = await _context.Orders.Where(o => o.Id == id).FirstOrDefaultAsync();
            List<OrderItem> orderItems = await _context.OrderItems.Where(o => o.OrderId == order.Id)
                .Include(p => p.Product).ToListAsync();
            AppUser user = await _usermanager.Users.FirstOrDefaultAsync(i => i.Id == order.AppUserId);
            OrderItemVM orderItemVM = new OrderItemVM();
            orderItemVM.User = user;
            orderItemVM.Order = order;
            orderItemVM.OrderItems = orderItems;
            return View(orderItemVM);
        }

        public async Task<IActionResult> Products()
        {
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            
            UserProductsVM userProductsVM = new UserProductsVM();
            userProductsVM.Products = await _context.Products.Where(p => p.AppUserId == user.Id)
                .Include(c => c.Category).Include(p => p.ProductImages).Include(t => t.ProductTags)
                .ToListAsync();
            return View(userProductsVM);
        }
        public async Task<IActionResult> CreateProduct()
        {
            var mainCategories = await _context.Categories.Where(p => p.ParentId == null).Where(p => p.IsDeleted != true).ToListAsync();
            var altCategories = await _context.Categories.Where(c => c.ParentId != null).Where(p => p.IsDeleted != true).ToListAsync();
            ViewBag.altCategories = new SelectList((altCategories).ToList(), "Id", "Name");
            ViewBag.Categories = new SelectList(_context.Categories.Where(c => c.IsDeleted != true).Where(c => c.ParentId == null).ToList(), "Id", "Name");
            ViewBag.Brands = new SelectList(_context.Brands.Where(c => c.IsDeleted != true).ToList(), "Id", "Name");
            ViewBag.Tags = new SelectList(_context.Tags.Where(t => t.IsDeleted != true).ToList(), "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductCreateVM vm)
        {
            var mainCategories = await _context.Categories.Where(p => p.ParentId == null).Where(p => p.IsDeleted != true).ToListAsync();
            var altCategories = await _context.Categories.Where(c => c.ParentId != null).Where(p => p.IsDeleted != true).ToListAsync();
            ViewBag.altCategories = new SelectList((altCategories).ToList(), "Id", "Name");
            ViewBag.Categories = new SelectList(_context.Categories.Where(c => c.IsDeleted != true).Where(c => c.ParentId == null).ToList(), "Id", "Name");
            ViewBag.Brands = new SelectList(_context.Brands.Where(c => c.IsDeleted != true).ToList(), "Id", "Name");
            ViewBag.Tags = new SelectList(_context.Tags.Where(t => t.IsDeleted != true).ToList(), "Id", "Name");
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Fill the required contents");
                return View(vm);
            }

            List<ProductImage> Images = new List<ProductImage>();
            foreach (var item in vm.Photos)
            {
                if (item == null)
                {
                    ModelState.AddModelError("Photo", "Do not leave it empty");
                    return View();
                }
                if (!item.IsImage())
                {
                    ModelState.AddModelError("Photo", "Do not leave it empty");
                    return View();
                }
                if (item.ValidSize(10000))
                {
                    ModelState.AddModelError("Photo", "Image size can not be large");
                    return View();
                }
                ProductImage image = new ProductImage();
                image.ImageUrl = item.SaveImage(_env, "images/products");
                Images.Add(image);
            }
            Product product = new Product
            {
                Name = vm.Name,
                AppUserId = user.Id,
                Price = vm.Price,
                Description = vm.Description,
                ReleaseDate = vm.ReleaseDate,
                OperationSystem = vm.OperationSystem,
                GPU = vm.GPU,
                Chipset = vm.Chipset,
                Memory = vm.Memory,
                Body = vm.Body,
                Display = vm.Display,
                FrontCamera = vm.FrontCamera,
                RearCamera = vm.RearCamera,
                Battery = vm.Battery,
                Weight = vm.Weight,
                DiscountPercent = vm.DiscountPercent,
                DiscountPrice = vm.Price - (vm.Price * vm.DiscountPercent) / 100,
                CategoryId = vm.CategoryId,
                BrandId = vm.BrandId,
                Count = vm.Count,
                Sold = 0,
                Profit = 0,
                CreatedTime = DateTime.Now,
                InStock = true,
                Views = 0
            };
            product.ProductImages = Images;
            product.ProductImages[0].IsMain = true;
            /*List<ProductTag> tags = new List<ProductTag>();
            foreach (var item in vm.TagId)
            {
                ProductTag tag = new ProductTag();
                tag.TagId = item;
                tag.ProductId = product.Id;
                tags.Add(tag);
            }
            product.ProductTags = tags;*/
            _context.Add(product);
            _context.SaveChanges();
            
            return RedirectToAction("Products");
        }
        public async Task<IActionResult> ProductDetail(int id)
        {
            return View();
        }
        public IActionResult GetSubCategory(int cid)
        {
            var SubCategory_List = _context.Categories
                .Where(s => s.ParentId == cid).Where(s => s.ParentId != null)
                .Select(c => new { Id = c.Id, Name = c.Name }).ToList();
            return Json(SubCategory_List);
        }
    }
}
