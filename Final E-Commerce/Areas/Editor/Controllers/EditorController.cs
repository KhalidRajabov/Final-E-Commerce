using Final_E_Commerce.Areas.Admin.ViewModels;
using Final_E_Commerce.Areas.Editor.ViewModels;
using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.Extensions;
using Final_E_Commerce.Helper;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using static System.Net.WebRequestMethods;

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
            UserBlogVM? editorVM = new UserBlogVM
            {
                Blogs = await _context?.Blogs?.OrderByDescending(b=>b.Date).ToListAsync()
            };
            return View(editorVM);
        }
        [HttpGet]
        public IActionResult NewBlog()
        {
            ViewBag.Subjects = new SelectList(_context?.Subjects?.Where(s => s.IsDeleted != true).ToList(), "Id", "Name");
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
            NewBlog.AppUserId = user.Id;
            NewBlog.Date = DateTime.Now;
            NewBlog.CommentCount = 0;
            NewBlog.ImageUrl = blog.Photo.SaveImage(_env, "images/blog");
            if (blog.SubjectId!= null)
            {
                List<BlogSubject> blogSubjects = new List<BlogSubject>();
                foreach (int item in blog.SubjectId)
                {
                    BlogSubject blogSubject = new BlogSubject();
                    blogSubject.SubjectsId = item;
                    blogSubject.BlogId = NewBlog.Id;
                    blogSubjects.Add(blogSubject);
                }
                NewBlog.BlogSubjects= blogSubjects;
            }
            await _context.AddAsync(NewBlog);
            await _context.SaveChangesAsync();
            List<Subscriber>? subscribers = await _context?.Subscribers?.ToListAsync();
            var token = "";
            string subject = $"{NewBlog.Title}";
            EmailHelper helper = new EmailHelper(_config.GetSection("ConfirmationParam:Email").Value, _config.GetSection("ConfirmationParam:Password").Value);
            foreach (var receiver in subscribers)
            {
                token = $"{NewBlog.Content.Substring(0,50)}... <a style='color: red' href='http://dante666-001-site1.atempurl.com/blog/detail/{NewBlog.Id}'>read more</a> ";
                var emailResult = helper.SendNews(receiver.Email, token, subject);
                continue;
            }
            string? discountemail = Url.Action("ConfirmEmail", "Account", new
            {
                token
            }, Request.Scheme);
            
            return RedirectToAction("index");
        }
        public async Task<IActionResult> BlogDetail(int id)
        {
            Blogs? blog = await _context?.Blogs?
                .Include(b => b.BlogSubjects)
                .ThenInclude(bs => bs.Subjects)
                .Include(bs => bs.Comments)
                .FirstOrDefaultAsync(b => b.Id == id);
            UserBlogDetailVM blogDetailVM = new UserBlogDetailVM();
            blogDetailVM.Blog = blog;
            return View(blogDetailVM); 
        }
        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Subjects = new SelectList(_context?.Subjects?.Where(s => s.IsDeleted != true).ToList(), "Id", "Name");
            Blogs? blog = await _context?.Blogs?.FirstOrDefaultAsync(b => b.Id == id);
            BlogEditVM editVM = new BlogEditVM();
            editVM.Title = blog.Title;
            editVM.Content = blog.Content;
            return View(editVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, BlogEditVM editVM)
        {
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            Blogs? blog = await _context?.Blogs?
                .Include(b=>b.BlogSubjects)
                .ThenInclude(b=>b.Subjects)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (editVM.Photo== null)
            {
                blog.ImageUrl = blog.ImageUrl;
            }
            else
            {
                if (!editVM.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Choose images only");
                    return View();
                }
                if (editVM.Photo.ValidSize(20000))
                {
                    ModelState.AddModelError("Photo", "Image size can not be large");
                    return View();
                }
                string? oldImg = blog.ImageUrl;
                string? path = Path.Combine(_env.WebRootPath, "images", oldImg);
                Helper.Helper.DeleteImage(path);
                blog.ImageUrl = editVM.Photo.SaveImage(_env, "images/blog");
            }
            blog.Title=editVM.Title;
            blog.Content = editVM.Content;
            blog.LastUpdated = DateTime.Now;
            blog.LastUpdatedBy = user.Fullname;
            if (editVM.SubjectId== null)
            {
                foreach (var item1 in blog.BlogSubjects)
                {
                    item1.SubjectsId = item1.SubjectsId;
                }
            }
            else
            {
                List<BlogSubject> blogSubjects = new List<BlogSubject>();
                foreach (int item in editVM.SubjectId)
                {
                    BlogSubject blogSubject = new BlogSubject();
                    blogSubject.SubjectsId = item;
                    blogSubject.BlogId = blog.Id;
                    blogSubjects.Add(blogSubject);
                }
                blog.BlogSubjects= blogSubjects;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("index","editor");
        }
        public async Task<IActionResult> DeleteBlog(int id)
        {
            Blogs? blog = await _context?.Blogs?.FirstOrDefaultAsync(b=>b.Id==id);
            blog.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }
        public async Task<IActionResult> RecoverBlog(int id)
        {
            Blogs? blog = await _context?.Blogs?.FirstOrDefaultAsync(b => b.Id == id);
            blog.IsDeleted = false;
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }
        public async Task<IActionResult> SearchBlog(string search)
        {
            List<Blogs>? blogs =await _context?.Blogs?
               .Where(b =>
               b.Title.ToLower().Contains(search.ToLower()) ||
               b.Content.ToLower().Contains(search.ToLower()))
               ?.Include(b => b.BlogSubjects)
               .ThenInclude(b=>b.Subjects)
               .OrderBy(p => p.Date)
               .Take(10).ToListAsync();
            if (blogs == null)
            {
                return RedirectToAction("error", "home");
            }
            UserBlogDetailVM detailVM = new UserBlogDetailVM();
            detailVM.Blogs = blogs;

            return PartialView("_BlogSearch", detailVM);
        }
    }
}