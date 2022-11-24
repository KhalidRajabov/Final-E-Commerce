using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.Extensions;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            List<Slider>? slider = _context?.Sliders?.ToList();
            ListSliderVM sliderVM = new ListSliderVM
            {
                Sliders = slider
            };
            return View(sliderVM);
        }

        public IActionResult Create()
        {
            if (_context?.Sliders?.Count() >= 6)
            {
                return RedirectToAction("index");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SliderVM slider)
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
                ImageUrl = slider.Photo.SaveImage(_env, "images/Slider"),
                FirstTitle = slider.FirstTitle,
                Subtitle = slider.Subtitle,
                MainTitle = slider.MainTitle,
                Description = slider.Description,
                Link = slider.Link
                
            };

            _context.Sliders.Add(newslider);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return RedirectToAction("error", "home");
            Slider? slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return RedirectToAction("error", "home");
            SliderVM sliderVM = new SliderVM
            {
                Id = slider.Id,
                FirstTitle = slider.FirstTitle,
                MainTitle = slider.MainTitle,
                Subtitle = slider.Subtitle,
                Description = slider.Description,
                ImageUrl = slider.ImageUrl,
                Link=slider.Link
            };
            return View(sliderVM);
        }


        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            Slider? slider = await _context?.Sliders?.FirstOrDefaultAsync(s => s.Id == id);
            SliderVM sliderVM = new SliderVM 
            {
                FirstTitle = slider.FirstTitle,
                MainTitle=slider.MainTitle,
                Subtitle=slider.Subtitle,
                Description=slider.Description,
                Link = slider.Link
            };
            return View(sliderVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id,SliderVM? slider)
        {
            Slider? NewSlider =await _context?.Sliders?.FirstOrDefaultAsync(s => s.Id == id);
            if (slider.Photo != null)
            {
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
                NewSlider.ImageUrl = slider?.Photo?.SaveImage(_env, "images/slider");
            }

            NewSlider.Subtitle = slider.Subtitle;
            NewSlider.FirstTitle = slider.FirstTitle;
            NewSlider.MainTitle = slider.MainTitle;
            NewSlider.Description = slider.Description;
            NewSlider.Link = slider.Link;
            await _context.SaveChangesAsync();

            return RedirectToAction("index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return RedirectToAction("error", "home");
            Slider? slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return RedirectToAction("error", "home");
            string? path = Path.Combine(_env.WebRootPath, "img", slider.ImageUrl);
            Helper.Helper.DeleteImage(path);
            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }
    }
}
