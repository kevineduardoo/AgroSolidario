using Microsoft.AspNetCore.Mvc;

namespace AgroSolidario.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}