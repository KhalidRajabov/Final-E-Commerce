using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.İnterfaces;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Final_E_Commerce.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser>? _usermanager;
        private readonly SignInManager<AppUser>? _signInManager;
        private IConfiguration _config { get; }

        public HomeRepository(AppDbContext context, UserManager<AppUser>? usermanager, SignInManager<AppUser>? signInManager, IConfiguration config)
        {
            _context = context;
            _usermanager = usermanager;
            _signInManager = signInManager;
            _config = config;
        }
        public HomeVM Index(string userName)
        { 
            List<Products>? AllProducts = _context.Products?
                .Where(p => p.DiscountPercent > 0).ToList();

            foreach (var product in AllProducts)
            {
                if (product.DiscountUntil < DateTime.Now.AddHours(12))
                {
                    product.DiscountUntil = null;
                    product.DiscountPercent = 0;
                    product.DiscountPrice = 0;
                    _context?.SaveChanges();
                }
            }

            HomeVM? homeVM = new HomeVM();
            List<Products> Bestsellers = _context.Products
                .OrderByDescending(p => p.Sold).Take(8)
                .Where(p => p.Status == ProductConfirmationStatus.Approved && !p.IsDeleted).Include(p => p.ProductImages).ToList();
            if (userName!=null)
            {
                AppUser user = _usermanager.FindByNameAsync(userName).GetAwaiter().GetResult();
                var userroles = _usermanager.GetRolesAsync(user).GetAwaiter().GetResult();
                foreach (var item in userroles)
                {
                    if (item.ToLower() == "ban" || userroles == null)
                    {
                        _signInManager.SignOutAsync();
                    }
                }
                homeVM.User = user;
                homeVM.Wishlists = _context?.Wishlists?.Where(w => w.AppUserId == user.Id).ToList();
                List<Products> Following = new List<Products>();
                List<UserSubscription>? followings = _context?.Subscription?.Where(s => s.SubscriberId == user.Id).ToList();
                foreach (var item in followings)
                {
                    AppUser profile = _usermanager.FindByIdAsync(item.ProfileId).GetAwaiter().GetResult();
                    List<Products>? ProfileProducts = _context.Products.Where(p => p.AppUserId == profile.Id)
                        .Include(p => p.ProductImages).ToList();
                    foreach (var products in ProfileProducts)
                    {
                        Following.Add(products);
                    }
                }
                homeVM.Following = Following;
            }
            homeVM.Bio = _context?.Bios?.FirstOrDefault();
            homeVM.Category = _context.Categories?.FirstOrDefault(c => c.Id == 1);
            homeVM.MostPopularProduct = _context.Products
                .Where(p => p.Status == ProductConfirmationStatus.Approved && !p.IsDeleted)
                .OrderByDescending(p => p.Rating).Take(1).Include(p => p.ProductImages).FirstOrDefault();
            homeVM.PopularProducts = _context.Products
                .Where(p => p.Status == ProductConfirmationStatus.Approved)
                .OrderByDescending(p => p.Rating).Skip(1).Take(3).Include(p => p.ProductImages).ToList();
            homeVM.BestSellerProducts = Bestsellers;
            homeVM.Sliders = _context?.Sliders?.ToList();
            homeVM.Blogs = _context?.Blogs?.Where(b => !b.IsDeleted).OrderByDescending(b => b.ViewCount).Take(4).ToList(); 
            return homeVM;
        }

        
    }
}
