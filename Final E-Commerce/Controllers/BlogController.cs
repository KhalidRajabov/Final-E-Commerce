using Final_E_Commerce.Areas.Editor.ViewModels;
using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.ViewModels;
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
        public BlogController(AppDbContext context, IWebHostEnvironment env, IConfiguration config, UserManager<AppUser> usermanager)
        {
            _context = context;
            _env = env;
            _config = config;
            _usermanager = usermanager;
        }


        public IActionResult Index()
        {
            BlogDetailVM blog = new BlogDetailVM
            {
                Blogs=_context.Blogs.OrderByDescending(b=>b.Id).Include(b=>b.BlogSubjects).ThenInclude(bs=>bs.Subjects).ToList(),
            };
            return View(blog);
        }
        public async Task<IActionResult> Detail(int? id)
        {
            Blogs dbBlog = await _context?.Blogs?.Include(b=>b.Comments).Include(b => b.BlogSubjects).ThenInclude(bs => bs.Subjects).FirstOrDefaultAsync(b => b.Id == id);
            dbBlog.ViewCount++;
            BlogVM? blog = new BlogVM
            {
                Blog =dbBlog
            };
            await _context.SaveChangesAsync();
            return View(blog);
        }
        [HttpPost]
        public async Task<IActionResult> PostComment(int id, BlogVM comment)
        {
            Blogs blog =await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
            BlogComment NewComment = new BlogComment();
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
                NewComment.Author = user.Fullname;
                NewComment.CommentContent = comment.CommentContent;
                NewComment.BlogId = blog.Id;
                NewComment.Date = DateTime.Now;
            }
            else
            {
                NewComment.Author = comment.Author;
                NewComment.CommentContent = comment.CommentContent;
                NewComment.BlogId = blog.Id;
                NewComment.Date = DateTime.Now;
            }
            await _context.AddAsync(NewComment);
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }
    }
}
