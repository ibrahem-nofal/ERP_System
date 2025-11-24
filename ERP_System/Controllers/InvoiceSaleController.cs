using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP_System.Models;
using ERP_System.Data;

namespace ERP_System.Controllers
{
    [Authorize]
    public class InvoiceSaleController : Controller
    {
        private readonly AppDbContext _context;

        public InvoiceSaleController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> List()
        {
            // Placeholder: Fetch invoices
            // var invoices = await _context.InvoiceSaleHeaders.Include(i => i.Customer).ToListAsync();
            return View(new List<InvoiceSaleHeader>());
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
