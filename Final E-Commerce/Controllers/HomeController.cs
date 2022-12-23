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
            ShopVM shopVM = new ShopVM();
            if (filter != null)
            {
                var x = from y in _context.Products
                    .Include(p => p.ProductImages)
                    .Where(p => filter.Search != null ? p.Status == ProductConfirmationStatus.Approved && p.Name.ToLower().Contains(filter.Search.ToLower()) : p.Status == ProductConfirmationStatus.Approved)
                        select y;
                switch (filter.AlphabeticOrder)
                {
                    case "Z-A":
                        x = x.OrderByDescending(p => p.Name);
                       break;
                    case "A-Z":
                        x = x.OrderBy(p => p.Name);
                        break;
                }
                switch (filter.Price)
                {
                    case "Low":
                        x = x.OrderBy(p => p.Price);
                        break;
                    case "High":
                        x = x.OrderByDescending(p => p.Price);
                        break;
                }
                switch (filter.DateOrder)
                {
                    case "New to Old":
                        x = x.OrderByDescending(p => p.CreatedTime);
                        break;
                    case "Old to New":
                        x = x.OrderBy(p => p.CreatedTime);
                        break;
                }
                switch (filter.Speciality)
                {
                    case "Populars":
                        x = x.OrderByDescending(p => p.Views);
                        break;
                    case "Top sellers":
                        x = x.OrderByDescending(p => p.Sold);
                        break;
                }
                shopVM.Products = x.AsNoTracking().ToList();
            }
            else
            {
                shopVM.Products = await _context?.Products?
                    .Include(p => p.ProductImages).Where(p => p.Status == ProductConfirmationStatus.Approved).ToListAsync();
            }
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