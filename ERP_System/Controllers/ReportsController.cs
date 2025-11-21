using Microsoft.AspNetCore.Mvc;

namespace ERP_System.Controllers
{
    public class ReportsController : Controller
    {
        public IActionResult Sales()
        {
            return View();
        }

        public IActionResult Purchases()
        {
            return View();
        }

        public IActionResult Inventory()
        {
            return View();
        }
    }
}
