using Final_E_Commerce.Areas.Editor.ViewModels;
using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_E_Commerce.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private IConfiguration _config { get; }
        private readonly UserManager<AppUser> _usermanager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public BlogController(AppDbContext context, IWebHostEnvironment env, IConfiguration config, UserManager<AppUser> usermanager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _env = env;
            _config = config;
            _usermanager = usermanager;
            _roleManager = roleManager;
        }


        public IActionResult Index()
        {
            BlogDetailVM blog = new BlogDetailVM
            {
                Blogs=_context.Blogs.Where(b=>!b.IsDeleted).OrderByDescending(b=>b.Id).Include(b=>b.BlogSubjects).ThenInclude(bs=>bs.Subjects).ToList(),
            };
            return View(blog);
        }
        public async Task<IActionResult> Detail(int? id)
        {
            Blogs dbBlog = await _context?.Blogs?.Include(b=>b.Comments).Include(b => b.BlogSubjects).ThenInclude(bs => bs.Subjects).FirstOrDefaultAsync(b => b.Id == id);
            if (dbBlog.IsDeleted||dbBlog==null)
            {
                return RedirectToAction("error", "home");
            }
            if (User.Identity.IsAuthenticated)
            {
                AppUser user =await _usermanager.FindByNameAsync(User.Identity.Name);
                ViewBag.AppUserId = user.Id;
                int RightCounter = 0;
                var roles =await _usermanager.GetRolesAsync(user);
                foreach (var item in roles)
                {
                    if (item.ToLower().Contains("admin")|| item.ToLower().Contains("editor")|| item.ToLower().Contains("moderator"))
                    {
                        RightCounter++;
                    }
                }
                ViewBag.RightCounter = RightCounter;
            }
            dbBlog.ViewCount++;
            dbBlog.CommentCount = _context.BlogComments.Where(b => b.BlogId == dbBlog.Id).ToList().Count;
            BlogVM? blog = new BlogVM
            {
                Blog =dbBlog
            };
            await _context.SaveChangesAsync();
            return View(blog);
        }

        [HttpPost]
        public async Task<IActionResult> PostComment(int id, BlogVM comment, string? returnurl)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Fill required forms");
            }
            Blogs? blog =await _context?.Blogs?.FirstOrDefaultAsync(b => b.Id == id);
            BlogComment NewComment = new BlogComment();
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
                NewComment.Author = user.Fullname;
                NewComment.CommentContent = comment.CommentContent;
                NewComment.AppUserId = user.Id;
            }
            else
            {
                NewComment.Author = comment.Author;
                NewComment.CommentContent = comment.CommentContent;
            }
            NewComment.BlogId = blog.Id;
            NewComment.Date = DateTime.Now;
            await _context.AddAsync(NewComment);
            await _context.SaveChangesAsync();
            if (returnurl != null)
            {
                return Redirect(returnurl);
            }
            return RedirectToAction("index");
        }
        [Authorize]
        public async Task<IActionResult> DeleteComment(int id, string? returnurl)
        {
            AppUser user =await _usermanager.FindByNameAsync(User.Identity.Name);
            BlogComment? comment = await _context?.BlogComments?.FirstOrDefaultAsync(bc => bc.Id == id);
            if (comment.AppUserId == user.Id)
            {
                _context?.Remove(comment);
                await _context.SaveChangesAsync();
            }
            else
            {
                var roles = await _usermanager.GetRolesAsync(user);
                foreach (var item in roles)
                {
                    if (item.ToLower().Contains("admin") || item.ToLower().Contains("editor") || item.ToLower().Contains("moderator"))
                    {
                    
                        _context?.Remove(comment);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            if (returnurl != null)
            {
                return Redirect(returnurl);
            }
            return RedirectToAction("index");
        }
    }
}
