using Microsoft.AspNetCore.Mvc;

namespace ERP_System.Controllers
{
    public class AccountController : Controller
            {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Mock login logic
            if (username == "admin" && password == "admin")
            {
                // In a real app, you would set a cookie or session here
                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.Error = "اسم المستخدم أو كلمة المرور غير صحيحة";
            return View();
        }
    }
}
