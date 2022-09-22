using Final_E_Commerce.Areas.Admin.ViewModels;
using Final_E_Commerce.Areas.Editor.ViewModels;
using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.Extensions;
using Final_E_Commerce.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Final_E_Commerce.Areas.Editor.Controllers
{
    [Area("Editor")]
    [Authorize(Roles = "Admin, SuperAdmin, Editor")]
    public class EditorController:Controller
    {
        private readonly AppDbContext? _context;
        private readonly IWebHostEnvironment _env;
        private IConfiguration _config { get; }
        private readonly UserManager<AppUser> _usermanager;

        public EditorController(AppDbContext? context, IWebHostEnvironment env, IConfiguration config, UserManager<AppUser> usermanager)
        {
            _context = context;
            _env = env;
            _config = config;
            _usermanager = usermanager;
        }

        public async Task<IActionResult> Index()
        {
            EditorVM? editorVM = new EditorVM
            {
                Blogs = await _context?.Blogs?.ToListAsync()
            };
            return View(editorVM);
        }
        [HttpGet]
        public IActionResult NewBlog()
        {
            ViewBag.Subjects = new SelectList(_context.Subjects.Where(s => s.IsDeleted != true).ToList(), "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NewBlog(WriteBlogVM blog)
        {
            if (blog == null) return RedirectToAction("error", "home");
            if (blog.Photo == null)
            {
                ModelState.AddModelError("Photo", "Do not leave it empty");
                return View();
            }

            if (!blog.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Do not leave it empty");
                return View();
            }
            if (blog.Photo.ValidSize(10000))
            {
                ModelState.AddModelError("Photo", "Image size can not be large");
                return View();
            }
            
            AppUser user =await _usermanager.FindByNameAsync(User.Identity.Name);
            Blogs NewBlog = new Blogs();
            NewBlog.Title = blog.Title;
            NewBlog.Content = blog.Content;
            NewBlog.Author = user.Fullname;
            NewBlog.Date = DateTime.Now;
            NewBlog.CommentCount = 0;
            NewBlog.ImageUrl = blog.Photo.SaveImage(_env, "images/blog");
            if (blog.SubjectId!= null)
            {
                List<BlogSubjects> blogSubjects = new List<BlogSubjects>();
                foreach (int item in blog.SubjectId)
                {
                    BlogSubjects blogSubject = new BlogSubjects();
                    blogSubject.SubjectId = item;
                    blogSubject.BlogId = NewBlog.Id;
                    blogSubjects.Add(blogSubject);
                }
                NewBlog.BlogSubjects= blogSubjects;
            }
            await _context.AddAsync(NewBlog);
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }
        public async Task<IActionResult> BlogDetail(int id)
        {
            Blogs? blog = await _context?.Blogs?.Include(b => b.BlogSubjects).ThenInclude(bs => bs.Subjects).FirstOrDefaultAsync(b => b.Id == id);
            BlogDetailVM blogDetailVM = new BlogDetailVM();
            blogDetailVM.Blog = blog;
            return View(blogDetailVM); 
        }
    }
}