using Final_E_Commerce.Areas.Admin.ViewModels;
using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.Helper;
using Final_E_Commerce.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_E_Commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class MessageController : Controller
    {
        private readonly AppDbContext? _context;
        private readonly UserManager<AppUser>? _usermanager;
        private readonly SignInManager<AppUser>? _signInManager;
        private readonly IConfiguration _config;

        public MessageController(AppDbContext? context, UserManager<AppUser>? usermanager, SignInManager<AppUser>? signInManager, IConfiguration config)
        {
            _context = context;
            _usermanager = usermanager;
            _signInManager = signInManager;
            _config = config;
        }


        public async Task<IActionResult> Index()
        {
            AdminMessagesVM? adminMessagesVM = new AdminMessagesVM
            {
                Messages = await _context?.Messages?.OrderByDescending(m=>!m.IsAnswered).ToListAsync()
            };
            return View(adminMessagesVM);
        }

        public async Task<IActionResult> Detail(int id)
        {
            AdminMessagesVM? adminMessagesVM = new AdminMessagesVM
            {
                SingleMessage = await _context?.Messages?.FirstOrDefaultAsync(m=>m.Id==id)
            };
            return View(adminMessagesVM);
        }
        [HttpPost]
        public async Task<IActionResult> Reply(AdminMessagesVM messagevm, int id)
        {
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            Messages? message = await _context?.Messages?.FirstOrDefaultAsync(m => m.Id == id);
            message.Answer = messagevm.Reply;
            message.AnsweredDate = DateTime.Now;
            message.AnsweredBy = user.Fullname;
            message.IsAnswered = true;

            if (message.AppUserId!=null)
            {
                AppUser messageOwner = await _usermanager.FindByIdAsync(message.AppUserId);
                var token = "";
                string subject = $"Answer for {message.Subject}";
                EmailHelper helper = new EmailHelper(_config.GetSection("ConfirmationParam:Email").Value, _config.GetSection("ConfirmationParam:Password").Value);

                token = $"Salam. {messageOwner.Fullname}. Göndərdiyiniz {message.Id} nömrəli mesaj {user.Fullname} tərəfindən cavablandırılmışdır.<br/> <br/> \n" +
                    $"{messagevm.Reply} <br/><br/> \n" +
                    $"Hörmətlə <br/>\n" +
                    $"TechnoStore {DateTime.Now.Year}";
                var emailResult = helper.SendNews(message.Email, token, subject);

                string? discountemail = Url.Action("ConfirmEmail", "Account", new
                {
                    token
                }, Request.Scheme);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }

    }
}
