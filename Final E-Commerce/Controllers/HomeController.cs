using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using Final_E_Commerce.Helper;
using Final_E_Commerce.İnterfaces;

namespace Final_E_Commerce.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly IHomeRepository _homeRepository;
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser>? _usermanager;
        private readonly SignInManager<AppUser>? _signInManager;
        private IConfiguration _config { get; }
        public HomeController(IHomeRepository homeRepository, AppDbContext context, UserManager<AppUser>? usermanager, SignInManager<AppUser>? signInManager, IConfiguration config)
        {
            _homeRepository = homeRepository;
            _context = context;
            _usermanager = usermanager;
            _signInManager = signInManager;
            _config = config;
        }
        public async Task<IActionResult> Index()
        {
            return View(_homeRepository.Index(User.Identity.Name.ToString()));   
        }


        public async Task<IActionResult> Detail(int? id)
        {
            if (_homeRepository.ExistProducts(id))
            {
                return View(_homeRepository.Detail(id, User.Identity.Name.ToString()));
            }
            else
            {
                return RedirectToAction("error");
            }
        }


        [Authorize]
        public async Task<IActionResult> Rate(int Rating, int ProductId)
        {
            return Ok(_homeRepository.Rate(Rating, ProductId, User.Identity.Name.ToString()));
        }
        [Authorize]
        public async Task<IActionResult> RemoveRating(int id, string ReturnUrl)
        {
            return Redirect(_homeRepository.RemoveRating(id, ReturnUrl, User.Identity.Name.ToString()));
        }

        public IActionResult Error()
        {
            return View();
        }


        public async Task<IActionResult> Shop(ShopVM? filter)
        {

            #region Discount/Ban operation
            List<Products>? AllProducts = await _context.Products?
              .Where(p => p.DiscountPercent > 0).ToListAsync();

            foreach (var product in AllProducts)
            {
                if (product.DiscountUntil < DateTime.Now.AddHours(12))
                {
                    product.DiscountUntil = null;
                    product.DiscountPercent = 0;
                    product.DiscountPrice = 0;
                    await _context?.SaveChangesAsync();
                }
            }
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
                var userroles = await _usermanager.GetRolesAsync(user);
                foreach (var item in userroles)
                {
                    if (item.ToLower() == "ban" || userroles == null)
                    {
                        await _signInManager.SignOutAsync();
                        return RedirectToAction("error", "home");
                    }
                }
            }
            #endregion

            #region SelectItems
            ViewBag.AlphabeticOrder = new List<string>() { "A-Z", "Z-A" };
            ViewBag.DateOrder = new List<string>() { "New to Old", "Old to New" };
            ViewBag.SpecialOrder = new List<string>() { "Populars", "Top sellers" };
            ViewBag.Price = new List<string>() { "Low", "High" };
            #endregion
            List<Products> Products = new List<Products>();
            if (filter != null)
            {
                Products = await _context?.Products?
                    .Include(p => p.ProductImages).Where(p => p.Status == ProductConfirmationStatus.Approved).ToListAsync();
                if (filter.Search != null)
                {
                    Products.Where(p => p.Name.ToLower().Contains(filter.Search.ToLower()));
                }
                if (filter.AlphabeticOrder != null)
                {
                    if (filter.AlphabeticOrder == "A-Z")
                    {
                        Products.OrderBy(p => p.Name);
                    }
                    else
                    {
                        Products.OrderByDescending(p => p.Name);
                    }
                }
                if (filter.DateOrder != null)
                {
                    if (filter.DateOrder == "New to Old")
                    {
                        Products.OrderByDescending(p => p.CreatedTime);
                    }
                    else
                    {
                        Products.OrderBy(p => p.CreatedTime);
                    }
                }
                if (filter.Speciality != null)
                {
                    if (filter.Speciality == "Populars")
                    {
                        Products.OrderByDescending(p => p.Views);
                    }
                    else
                    {
                        Products.OrderByDescending(p => p.Sold);
                    }
                }
                if (filter.Price != null)
                {
                    if (filter.Speciality == "Low")
                    {
                        Products.OrderBy(p => p.Price);
                    }
                    else
                    {
                        Products.OrderByDescending(p => p.Price);
                    }
                }
            }
            else
            {
                Products = await _context?.Products?
                    .Include(p => p.ProductImages).Where(p => p.Status == ProductConfirmationStatus.Approved).ToListAsync();
            }
            ShopVM shopVM = new ShopVM
            {
                Products = Products
            };
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
                shopVM.User = user;
                shopVM.Wishlists = await _context?.Wishlists?.Where(w => w.AppUserId == user.Id).ToListAsync();
            }

            return View(shopVM);
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
                .Include(p => p.AppUser)
                .FirstOrDefaultAsync(p => p.Id == ProductId);
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
            NewComment.Date = DateTime.Now.AddHours(12);
            await _context.AddAsync(NewComment);
            await _context.SaveChangesAsync();
            commentVM.ProductComment = NewComment;
            return PartialView("_ProductSingleComment", commentVM);
        }
        [Authorize]
        public async Task<IActionResult> DeleteComment(int id)
        {
            return Ok(_homeRepository.DeleteComment(id, User.Identity.Name.ToString()));
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
            return View(_homeRepository.Brands(id, User.Identity.Name.ToString()));
        }

        public async Task<IActionResult> Contact()
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

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Contact(ContactVM contact)
        {
            Messages message = new Messages();
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
                message.AppUserId = user.Id;
                message.Firstname = user.Firstname;
                message.Lastname = user.Lastname;
                message.Email = user.Email;
            }
            else
            {
                message.Firstname = contact.Firstname;
                message.Lastname = contact.Lastname;
                message.Email = contact.Email;
            }
            message.Subject = contact.Subject;
            message.Content = contact.Message;
            message.Date = DateTime.Now.AddHours(12);
            await _context.AddAsync(message);
            await _context.SaveChangesAsync();
            #region
            List<AppUser> AppUsers = await _usermanager.Users.ToListAsync();
            foreach (var user in AppUsers)
            {
                var userroles = await _usermanager.GetRolesAsync(user);
                foreach (var item in userroles)
                {
                    if (item.ToLower() == "admin")
                    {
                        var token = "";
                        string subject = $"New message about {contact.Subject}";
                        EmailHelper helper = new EmailHelper(_config.GetSection("ConfirmationParam:Email").Value, _config.GetSection("ConfirmationParam:Password").Value);

                        token = $"Hello {user.Fullname}. {contact.Firstname} {contact.Lastname} asked something <br> <br> \n" +
                            "Message: <br>\n" +
                            $"<p style='align-text: center'>{contact.Message}</p> <br>\n" +
                            $"Go to the app: <a href='https://localhost:44393/admin/message/detail/{message.Id}' style='color:red'>{message.Subject} </a>";
                        var emailResult = helper.SendNews(user.Email, token, subject);

                        string? discountemail = Url.Action("ConfirmEmail", "Account", new
                        {
                            token
                        }, Request.Scheme);
                    }
                }
            }
            #endregion
            return View();
        }
    }
}