using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP_System.Models;
using ERP_System.Data;

namespace ERP_System.Controllers
{
    [Authorize]
    public class InventoryController : Controller
    {
        private readonly AppDbContext _context;

        public InventoryController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> List()
        {
            var inventory = await _context.Inventories
                .Include(i => i.Item)
                .Include(i => i.Store)
                .OrderBy(i => i.Item.Name)
                .ToListAsync();
            return View(inventory);
        }

        public async Task<IActionResult> Details(int storeId, int itemId)
        {
            var inventory = await _context.Inventories
                .Include(i => i.Item)
                .Include(i => i.Store)
                .FirstOrDefaultAsync(i => i.StoreId == storeId && i.ItemId == itemId);

            if (inventory == null) return NotFound();

            return View(inventory);
        }
    }
}
