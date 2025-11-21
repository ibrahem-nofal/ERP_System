using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP_System.Models;
using ERP_System.Data;

namespace ERP_System.Controllers
{
    public class InvoicePurchaseController : Controller
    {
        private readonly AppDbContext _context;

        public InvoicePurchaseController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> List()
        {
            // Placeholder
            return View(new List<InvoicePurchaseHeader>());
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
