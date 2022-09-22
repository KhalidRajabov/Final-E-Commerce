using Final_E_Commerce.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Final_E_Commerce.Areas.Editor.Controllers
{
    [Area("Editor")]
    [Authorize(Roles = "Admin, SuperAdmin, Editor")]
    public class MainController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
