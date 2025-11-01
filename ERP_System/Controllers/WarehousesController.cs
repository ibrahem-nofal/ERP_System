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

            // حساب إجمالي عدد المنتجات لجميع المخازن
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
                        .ThenInclude(p => p.Category)
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

                TempData["Success"] = "تمت إضافة المخزن بنجاح ✅";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "حدث خطأ أثناء حفظ البيانات ❌";
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

                    TempData["Success"] = "تم تعديل بيانات المخزن بنجاح ✅";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Warehouses.Any(w => w.Id == warehouse.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "حدث خطأ أثناء التعديل ❌";
            return View(warehouse);
        }

        // 🟡 صفحة تأكيد الحذف
        public async Task<IActionResult> Delete(int id)
        {
            var warehouse = await _context.Warehouses
                .FirstOrDefaultAsync(w => w.Id == id);

            if (warehouse == null)
                return NotFound();

            return View(warehouse);
        }

        // 🔴 تنفيذ الحذف
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse == null)
                return NotFound();

            _context.Warehouses.Remove(warehouse);
            await _context.SaveChangesAsync();

            TempData["Success"] = "تم حذف المخزن بنجاح 🗑️";
            return RedirectToAction(nameof(Index));
        }
    }
}
