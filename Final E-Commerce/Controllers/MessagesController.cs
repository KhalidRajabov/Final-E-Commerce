using Microsoft.AspNetCore.Mvc;

namespace Final_E_Commerce.Controllers
{
    public class MessagesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
