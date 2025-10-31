using ERP_System.Data;
using ERP_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP_System.Controllers
{
    public class WarehousesController : Controller
    {
        private readonly AppDbContext _context;

        public WarehousesController(AppDbContext context)
        {
            _context = context;
        }

        // 🟢 عرض كل المخازن
        public async Task<IActionResult> Index()
        {
            var warehouses = await _context.Warehouses
                .Include(w => w.StockLevels)
                .ThenInclude(sl => sl.Product)
                .ToListAsync();

            // حساب نسبة الإشغال لكل مخزن (عدد المنتجات)
            var totalProducts = await _context.Products.CountAsync();
            ViewBag.TotalProducts = totalProducts;

            return View(warehouses);
        }

        // 🟢 عرض تفاصيل مخزن معين
        public async Task<IActionResult> Details(int id)
        {
            var warehouse = await _context.Warehouses
                .Include(w => w.StockLevels)
                .ThenInclude(sl => sl.Product)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (warehouse == null)
                return NotFound();

            return View(warehouse);
        }

        // 🟡 صفحة الإضافة
        public IActionResult Create()
        {
            return View();
        }

        // 🟢 تنفيذ الإضافة
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                _context.Warehouses.Add(warehouse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(warehouse);
        }

        // 🟡 صفحة التعديل
        public async Task<IActionResult> Edit(int id)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse == null)
                return NotFound();

            return View(warehouse);
        }

        // 🟢 تنفيذ التعديل
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Warehouse warehouse)
        {
            if (id != warehouse.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(warehouse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Warehouses.Any(w => w.Id == warehouse.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(warehouse);
        }

        // 🔴 حذف المخزن
        public async Task<IActionResult> Delete(int id)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse == null)
                return NotFound();

            _context.Warehouses.Remove(warehouse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
