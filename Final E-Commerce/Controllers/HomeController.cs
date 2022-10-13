using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.CodeAnalysis;
using Final_E_Commerce.Migrations;

namespace Final_E_Commerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext? _context;
        private readonly UserManager<AppUser>? _usermanager;
        private readonly SignInManager<AppUser>? _signInManager;
        public HomeController(AppDbContext? context, UserManager<AppUser>? usermanager, SignInManager<AppUser>? signInManager)
        {
            _context = context;
            _usermanager = usermanager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Index()
        {
            List<Products>? AllProducts = await _context.Products?
                .Where(p=>p.DiscountPercent>0).ToListAsync();

            if (User.Identity.IsAuthenticated)
            {
                AppUser AppUser = await _usermanager.FindByNameAsync(User.Identity.Name);
                var userroles = await _usermanager.GetRolesAsync(AppUser);
                foreach (var item in userroles)
                {
                    if (item.ToLower() == "ban" || userroles == null)
                    {
                        await _signInManager.SignOutAsync();
                        return RedirectToAction("error", "home");
                    }
                }
            }
            foreach (var product in AllProducts)
            {

                if (product.DiscountUntil<DateTime.Now)
                {
                    product.DiscountUntil = null;
                    product.DiscountPercent = 0;
                    product.DiscountPrice = 0;
                    _context?.SaveChangesAsync();
                }
            }
            
            HomeVM? homeVM = new HomeVM();
            List<Products> Bestsellers = await _context.Products
                .OrderByDescending(p => p.Sold).Take(8)
                .Where(p => p.Status == ProductConfirmationStatus.Approved).Include(p => p.ProductImages).ToListAsync();
            if (User.Identity.IsAuthenticated)
            {
                
                AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
                homeVM.User = user;
                homeVM.Wishlists = await _context?.Wishlists?.Where(w => w.AppUserId == user.Id).ToListAsync(); ;

            }
            homeVM.Bio = await _context?.Bios?.FirstOrDefaultAsync();
            homeVM.Category =await _context.Categories?.FirstOrDefaultAsync(c=>c.Id==1);
            homeVM.MostPopularProduct = await _context.Products
                .Where(p => p.Status == ProductConfirmationStatus.Approved)
                .OrderByDescending(p=>p.Rating).Take(1).Include(p=>p.ProductImages).FirstOrDefaultAsync();
            homeVM.PopularProducts = await _context.Products
                .Where(p => p.Status == ProductConfirmationStatus.Approved)
                .OrderByDescending(p=>p.Rating).Skip(1).Take(3).Include(p=>p.ProductImages).ToListAsync();
            homeVM.BestSellerProducts = Bestsellers;
            homeVM.Sliders = await _context?.Sliders?.ToListAsync();
            return View(homeVM);
        }
        public async Task<IActionResult> Detail(int? id)
        {
            List<Products>? AllProducts = await _context.Products?
               .Where(p => p.DiscountPercent > 0).ToListAsync();
            foreach (var item in AllProducts)
            {
                if (item.DiscountUntil < DateTime.Now)
                {
                    item.DiscountUntil = null;
                    item.DiscountPercent = 0;
                    item.DiscountPrice = 0;
                    await _context.SaveChangesAsync();
                }
            }
            if (User.Identity.IsAuthenticated)
            {
                AppUser AppUser = await _usermanager.FindByNameAsync(User.Identity.Name);
                var userroles = await _usermanager.GetRolesAsync(AppUser);
                foreach (var item in userroles)
                {
                    if (item.ToLower() == "ban" || userroles == null)
                    {
                        await _signInManager.SignOutAsync();
                        return RedirectToAction("error", "home");
                    }
                }
            }
            DetailVM? detailVM = new DetailVM();
            Products? product = await _context?.Products?
                .Where(p=>p.Status==ProductConfirmationStatus.Approved)
                ?.Include(p => p.ProductImages)
                ?.Include(c => c.Category)
                ?.Include(p => p.Brand)
                ?.Include(p => p.ProductTags)
                ?.ThenInclude(t => t.Tags)
                ?.Include(r=>r.UserProductRatings)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product==null)
            {
                return RedirectToAction("error");
            }
            double rates = 0;
            List<UserProductRatings>? ratings = await _context?.UserProductRatings?
                .Where(r => r.ProductId == product.Id).ToListAsync();
            if (ratings.Count>=1)
            {
                foreach (var item in ratings)
                {
                    rates += item.Rating;
                }
                product.Rating = rates /ratings.Count;
                ViewBag.Just = rates / ratings.Count;
                ViewBag.RatedBy = ratings.Count;
                await _context.SaveChangesAsync();
            }
            if (product == null) return RedirectToAction("Error", "home");
            AppUser ProductOwner =await _usermanager.FindByIdAsync(product.AppUserId);
            if (ProductOwner!=null)
            {
                detailVM.Owner = ProductOwner;
            }
            product.CommentCount = _context.ProductComments.Where(p => p.ProductId == product.Id && !p.IsDeleted).ToList().Count;
            ViewBag.ExistWishlist = false;
            ViewBag.IsRated = false;
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
                bool IsExist = await _context.Wishlists.Where(w => w.AppUserId == user.Id && w.ProductId == id).AnyAsync();
                if (IsExist) ViewBag.ExistWishlist = true;
                
                bool IsRated = await _context.UserProductRatings
                    .Where(r => r.ProductId == product.Id && r.AppUserId == user.Id).AnyAsync();
                if (IsRated) ViewBag.IsRated = true;
                ViewBag.AppUserId = user.Id;
                int RightCounter = 0;
                var roles = await _usermanager.GetRolesAsync(user);
                foreach (var item in roles)
                {
                    if (item.ToLower().Contains("admin") || item.ToLower().Contains("editor") || item.ToLower().Contains("moderator"))
                    {
                        RightCounter++;
                    }
                }
                ViewBag.RightCounter = RightCounter;
            }
            product.Views++;
            await _context.SaveChangesAsync();
            var UsersWantThis = await _context.Wishlists?.Where(p=>p.ProductId==id).ToListAsync();
            detailVM.Product = product;

            List<Products> related = await _context.Products
                .Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id)
                .Include(p => p.ProductImages)
                .Take(4)
                .ToListAsync();

            detailVM.RelatedProducts = related;
            

            detailVM.UsersWantIt = UsersWantThis.Count;
            
            detailVM.Comments= await _context.ProductComments
                .Include(b => b.User)
                .Where(c => c.ProductId == id && !c.IsDeleted)
                .OrderByDescending(b => b.Id)
                .Take(10)
                .ToListAsync();

            return View(detailVM);
        }

        [Authorize]
        public async Task<IActionResult> Rate(int Rating, int ProductId)
        {
            bool result = false;
            string? ProductName = "";
            string? ProductImage = "";
            if (User.Identity.IsAuthenticated)
            {
                AppUser AppUser = await _usermanager.FindByNameAsync(User.Identity.Name);
                var userroles = await _usermanager.GetRolesAsync(AppUser);
                foreach (var item in userroles)
                {
                    if (item.ToLower() == "ban" || userroles == null)
                    {
                        await _signInManager.SignOutAsync();
                        return RedirectToAction("error", "home");
                    }
                }
            }
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
                Products? product = await _context?.Products?
                    .Include(p=>p.ProductImages).FirstOrDefaultAsync(p=>p.Id == ProductId);
                UserProductRatings userProductRating = new UserProductRatings
                {
                    AppUserId = user.Id,
                    ProductId=product.Id,
                    Rating = Rating
                };
                _context.Add(userProductRating);
                _context.SaveChanges();
                result = true;
                ProductName = product.Name;
                foreach (var item in product.ProductImages)
                {
                    if (item.IsMain)
                    {
                        ProductImage = item.ImageUrl;
                        break;
                    }
                }
            }
            else
            {
                result = false;
            }
            var obj = new
            {
                result = result,
                image = ProductImage,
                name = ProductName
            };
            return Ok(obj);
        }
        [Authorize]
        public async Task<IActionResult> RemoveRating(int id, string ReturnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                AppUser AppUser = await _usermanager.FindByNameAsync(User.Identity.Name);
                var userroles = await _usermanager.GetRolesAsync(AppUser);
                foreach (var item in userroles)
                {
                    if (item.ToLower() == "ban" || userroles == null)
                    {
                        await _signInManager.SignOutAsync();
                        return RedirectToAction("error", "home");
                    }
                }
            }
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            UserProductRatings? rating = await _context?.UserProductRatings?.Where(r => r.AppUserId == user.Id && r.ProductId == id).FirstOrDefaultAsync();
            _context.UserProductRatings.Remove(rating);
            await _context?.SaveChangesAsync();
            return Redirect(ReturnUrl);
        }


        public IActionResult Error()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PostComment(int ProductId, string comment, string? author)
        {
            if (User.Identity.IsAuthenticated)
            {
                AppUser AppUser = await _usermanager.FindByNameAsync(User.Identity.Name);
                var userroles = await _usermanager.GetRolesAsync(AppUser);
                foreach (var item in userroles)
                {
                    if (item.ToLower() == "ban" || userroles == null)
                    {
                        await _signInManager.SignOutAsync();
                        return RedirectToAction("error", "home");
                    }
                }
            }
            Products? product = await _context?.Products?
                .FirstOrDefaultAsync(p=>p.Id == ProductId);
            ProductComment NewComment = new ProductComment();
            CommentsVM commentVM = new CommentsVM();
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
                NewComment.AppUserId = user.Id;
                commentVM.UserId = user.Id;
                commentVM.User = user;
            }
            else
            {
                NewComment.Author = author;
            }
            NewComment.Content = comment;
            NewComment.ProductId = product.Id;
            NewComment.Date = DateTime.Now;
            await _context.AddAsync(NewComment);
            await _context.SaveChangesAsync();
            commentVM.ProductComment= NewComment;
            
            return PartialView("_ProductSingleComment", commentVM);
        }
        [Authorize]
        public async Task<IActionResult> DeleteComment(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                AppUser AppUser = await _usermanager.FindByNameAsync(User.Identity.Name);
                var userroles = await _usermanager.GetRolesAsync(AppUser);
                foreach (var item in userroles)
                {
                    if (item.ToLower() == "ban" || userroles == null)
                    {
                        await _signInManager.SignOutAsync();
                        return RedirectToAction("error", "home");
                    }
                }
            }
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            ProductComment? comment = await _context?.ProductComments?.FirstOrDefaultAsync(bc => bc.Id == id);
            if (comment.AppUserId == user.Id)
            {
                comment.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
            else
            {
                var roles = await _usermanager.GetRolesAsync(user);
                foreach (var item in roles)
                {
                    if (item.ToLower().Contains("admin") || item.ToLower().Contains("editor") || item.ToLower().Contains("moderator"))
                    {
                        comment.IsDeleted = true;
                        await _context.SaveChangesAsync();
                    }
                }
            }

            var obj = new
            {
                count = _context.ProductComments.Where(b => b.ProductId == comment.ProductId && !b.IsDeleted).ToList().Count
            };

            return Ok(obj);
        }

        public async Task<IActionResult> LoadComments(int skip, int? BlogId)
        {
            if (User.Identity.IsAuthenticated)
            {
                AppUser AppUser = await _usermanager.FindByNameAsync(User.Identity.Name);
                var userroles = await _usermanager.GetRolesAsync(AppUser);
                foreach (var item in userroles)
                {
                    if (item.ToLower() == "ban" || userroles == null)
                    {
                        await _signInManager.SignOutAsync();
                        return RedirectToAction("error", "home");
                    }
                }
            }

            List<ProductComment>? comments = await _context?.ProductComments?
                .Include(b => b.User)
                .Where(bc => bc.ProductId == BlogId && !bc.IsDeleted)
                .OrderByDescending(b => b.Id).Skip(skip).Take(2).ToListAsync();
            CommentsVM commentsVM = new CommentsVM
            {
                ProductComments = comments
            };
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
                ViewBag.AppUserId = user.Id;
                int RightCounter = 0;
                var roles = await _usermanager.GetRolesAsync(user);
                //if requester is an admin or editor, he will be able to delete comment
                foreach (var item in roles)
                {
                    if (item.ToLower().Contains("admin") || item.ToLower().Contains("editor") || item.ToLower().Contains("moderator"))
                    {
                        RightCounter++;
                    }
                }

                //if requester is not an admin but a user and finds any of his comment among those,
                //he will be able to delete his own comment
                commentsVM.UserId = user.Id;


                commentsVM.RightCounter = RightCounter;
            }
            return PartialView("_ProductComments", commentsVM);
        }

        public async Task<IActionResult> Brands(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                AppUser AppUser = await _usermanager.FindByNameAsync(User.Identity.Name);
                var userroles = await _usermanager.GetRolesAsync(AppUser);
                foreach (var item in userroles)
                {
                    if (item.ToLower() == "ban" || userroles == null)
                    {
                        await _signInManager.SignOutAsync();
                        return RedirectToAction("error", "home");
                    }
                }
            }
            List<Products>? products = await _context?.Products?
                .Where(p=>p.BrandId==id)
                .Include(p => p.ProductImages)
                .Include(p=>p.Brand)
                .ToListAsync();
            ListProductsVM listProducts = new ListProductsVM
            {
                Products = products,
                Brand = await _context?.Brands?.FirstOrDefaultAsync(b=>b.Id==id)
            };
            return View(listProducts);
        }
    }
}