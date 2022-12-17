using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.Helper;
using Final_E_Commerce.İnterfaces;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult Index()
        {
            string? username= User.Identity.IsAuthenticated ? User.Identity.Name : null;
            return View(_homeRepository.Index(username));
        }


        public IActionResult Detail(int? id)
        {
            string? username = User.Identity.IsAuthenticated ? User.Identity.Name : null;
            if (!_homeRepository.ExistProducts(id))
            {
                return RedirectToAction("error");
            }
            return View(_homeRepository.Detail(id, username));
            
        }


        [Authorize]
        public IActionResult Rate(int Rating, int ProductId)
        {
            string? username = User.Identity.IsAuthenticated ? User.Identity.Name : null;
            return Ok(_homeRepository.Rate(Rating, ProductId, username));
        }
        [Authorize]
        public IActionResult RemoveRating(int id, string ReturnUrl)
        {
            string? username = User.Identity.IsAuthenticated ? User.Identity.Name : null;
            return Redirect(_homeRepository.RemoveRating(id, ReturnUrl, username));
        }

        [Authorize]
        public IActionResult DeleteComment(int id)
        {
            string? username = User.Identity.IsAuthenticated ? User.Identity.Name : null;
            return Ok(_homeRepository.DeleteComment(id, username));
        }
        public IActionResult Brands(int id)
        {
            string? username = User.Identity.IsAuthenticated ? User.Identity.Name : null;
            return View(_homeRepository.Brands(id, username));
        }

        public IActionResult Error()
        {
            return View();
        }



        public async Task<IActionResult> Shop(ShopVM? filter)
        {


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
        public IActionResult PostComment(int ProductId, string comment, string? author)
        {
            string? username = User.Identity.IsAuthenticated ? User.Identity.Name : null;
            return PartialView("_ProductSingleComment", _homeRepository.PostComment(ProductId, comment, author, username));
        }
        
        public IActionResult LoadComments(int skip, int? BlogId)
        {
            string? username = User.Identity.IsAuthenticated ? User.Identity.Name : null;
            return PartialView("_ProductComments", _homeRepository.LoadComments(skip, BlogId, username));
        }

        

        public IActionResult Contact()
        {
            string? username = User.Identity.IsAuthenticated ? User.Identity.Name : null;
            if (User.Identity.IsAuthenticated)
            {
                _homeRepository.UserBanned(username);
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