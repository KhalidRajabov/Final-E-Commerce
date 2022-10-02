using AutoMapper;
using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Authorization;

namespace Final_E_Commerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext? _context;
        private readonly UserManager<AppUser>? _usermanager;
        public HomeController(AppDbContext? context, UserManager<AppUser>? usermanager)
        {
            _context = context;
            _usermanager = usermanager;
        }
        public async Task<IActionResult> Index()
        {
            List<Product>? AllProducts = await _context.Products?
                .Where(p=>p.DiscountPercent>0).ToListAsync();
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
            homeVM.Bio = _context?.Bios?.FirstOrDefault();
            homeVM.Category = _context.Categories?.FirstOrDefault(c=>c.Id==1);
            homeVM.MostPopularProduct = _context.Products
                .Where(p => p.Status == ProductConfirmationStatus.Approved)
                .OrderByDescending(p=>p.Views).Take(1).Include(p=>p.ProductImages).FirstOrDefault();
            homeVM.PopularProducts = _context.Products
                .Where(p => p.Status == ProductConfirmationStatus.Approved)
                .OrderByDescending(p=>p.Views).Skip(1).Take(3).Include(p=>p.ProductImages).ToList();
            homeVM.BestSellerProducts = _context.Products
                .OrderByDescending(p => p.Sold).Take(8)
                .Where(p=>p.Status==ProductConfirmationStatus.Approved).Include(p => p.ProductImages).ToList();
            return View(homeVM);
        }
        public async Task<IActionResult> Detail(int? id)
        {
            List<Product>? AllProducts = await _context.Products?
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
            DetailVM? detailVM = new DetailVM();
            Product? product = await _context?.Products?
                .Where(p=>p.Status==ProductConfirmationStatus.Approved)
                ?.Include(p => p.ProductImages)
                ?.Include(c => c.Category)
                ?.Include(p => p.Brand)
                ?.Include(p => p.ProductTags)
                ?.ThenInclude(t => t.Tags)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return RedirectToAction("Error", "home");
            AppUser ProductOwner =await _usermanager.FindByIdAsync(product.AppUserId);
            if (ProductOwner!=null)
            {
                detailVM.Owner = ProductOwner;
            }
            product.CommentCount = _context.ProductComments.Where(p => p.ProductId == product.Id && !p.IsDeleted).ToList().Count;
            ViewBag.ExistWishlist = false;
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
                bool IsExist = _context.Wishlists.Where(w => w.AppUserId == user.Id && w.ProductId == id).Any();
                if (IsExist)
                {
                    ViewBag.ExistWishlist = true;
                }
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
            var UsersWantThis = _context.Wishlists?.Where(p=>p.ProductId==id).ToList();
            detailVM.Product = product;
            
            detailVM.UsersWantIt = UsersWantThis.Count;
            
            detailVM.Comments= _context.ProductComments
                .Include(b => b.User)
                .Where(c => c.ProductId == id && !c.IsDeleted)
                .OrderByDescending(b => b.Id)
                .Take(10)
                .ToList(); ;

            return View(detailVM);
        }
        public IActionResult Error()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PostComment(int ProductId, string comment, string? author)
        {
            Product? product = await _context?.Products?.FirstOrDefaultAsync(p=>p.Id == ProductId);
            ProductComment NewComment = new ProductComment();
            CommentsVM commentVM = new CommentsVM();
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
                NewComment.AppUserId = user.Id;
                commentVM.UserId = user.Id;
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

            List<ProductComment>? comments = _context?.ProductComments?
                .Include(b => b.User)
                .Where(bc => bc.ProductId == BlogId && !bc.IsDeleted)
                .OrderByDescending(b => b.Id).Skip(skip).Take(2).ToList();
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

    }
}