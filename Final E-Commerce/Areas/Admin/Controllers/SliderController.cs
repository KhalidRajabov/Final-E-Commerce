using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.Extensions;
using Final_E_Commerce.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Final_E_Commerce.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            List<Slider> slider = _context.Sliders.ToList();
            return View(slider);
        }

        public IActionResult Create()
        {
            if (_context.Sliders.Count() >= 6)
            {
                return RedirectToAction("index");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Slider slider)
        {

            if (slider.Photo == null)
            {
                ModelState.AddModelError("Photo", "Do not leave it empty");
                return View();
            }

            if (!slider.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Do not leave it empty");
                return View();
            }
            if (slider.Photo.ValidSize(10000))
            {
                ModelState.AddModelError("Photo", "Image size can not be large");
                return View();
            }



            Slider newslider = new Slider
            {
                ImageUrl = slider.Photo.SaveImage(_env, "images"),
                Subtitle = slider.Subtitle,
                MainTitle = slider.MainTitle,
                Description = slider.Description
            };

            _context.Sliders.Add(newslider);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return RedirectToAction("error", "home");
            Slider slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return RedirectToAction("error", "home");
            return View(slider);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return RedirectToAction("error", "home");
            Slider slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return RedirectToAction("error", "home");
            string path = Path.Combine(_env.WebRootPath, "img", slider.ImageUrl);
            Helper.Helper.DeleteImage(path);
            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }
    }
}
