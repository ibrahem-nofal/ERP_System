using Microsoft.AspNetCore.Mvc;

namespace ERP_System.Controllers
{
    public class BaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
