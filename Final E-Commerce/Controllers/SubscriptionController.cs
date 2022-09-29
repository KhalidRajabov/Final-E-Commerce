using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.Helper;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_E_Commerce.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private IConfiguration _config { get; }

        public SubscriptionController(UserManager<AppUser> userManager, AppDbContext context, IConfiguration config)
        {
            _userManager = userManager;
            _context = context;
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> Subscribe(string email)
        {
            List<Subscriber>? subscribers = await _context?.Subscribers?.ToListAsync();
            foreach (var subscriber in subscribers)
            {
                if (subscriber.Email==email)
                {
                    return StatusCode(200);
                }
            }
            Subscriber? newsubscriber = new Subscriber();
            newsubscriber.Email = email;
            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user!=null) user.Subscribed = true;
            await _context.AddAsync(newsubscriber);
            await _context.SaveChangesAsync();
            
            var token = "";
            string? subject = "Subscription!";
            EmailHelper helper = new EmailHelper(_config.GetSection("ConfirmationParam:Email").Value, _config.GetSection("ConfirmationParam:Password").Value);
            
                token = $"We are happy for your being interested our website. By subscribing to us, you will get notified for news about us.";
                var emailResult = helper.SendNews(email, token, subject);
            
            
            string? discountemail = Url.Action("ConfirmEmail", "Account", new
            {
                token
            }, Request.Scheme);

            return StatusCode(200);

        }
    }
}
