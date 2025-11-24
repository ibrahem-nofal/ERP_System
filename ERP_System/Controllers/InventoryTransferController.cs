using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ERP_System.Models;
using ERP_System.Data;

namespace ERP_System.Controllers
{
    [Authorize]
    public class InventoryTransferController : Controller
    {
        private readonly AppDbContext _context;

        public InventoryTransferController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult List()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
