using ERP_System.Data;
using ERP_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP_System.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // 🟢 عرض كل المنتجات
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.StockLevels)
                    .ThenInclude(sl => sl.Warehouse)
                .ToListAsync();

            ViewBag.TotalCount = products.Count;
            ViewBag.TotalValue = products.Sum(p => p.UnitPrice * (p.StockLevels?.Sum(sl => sl.Quantity) ?? 0));

            return View(products);
        }

        // 🟢 صفحة الإضافة
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        // 🟢 تنفيذ الإضافة
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            try
            {
                if (product.CategoryId == 0)
                {
                    var defaultCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Name == "افتراضي");
                    if (defaultCategory != null)
                        product.CategoryId = defaultCategory.Id;
                }

                product.Status = "غير متوفر";

                if (ModelState.IsValid)
                {
                    _context.Products.Add(product);
                    await _context.SaveChangesAsync();

                    // توليد الكود التلقائي داخل النظام (غير ظاهر للمستخدم)
                    product.Code = $"ITM-{product.Id:D3}";
                    _context.Update(product);
                    await _context.SaveChangesAsync();

                    // إضافة المنتج للمخزن الافتراضي بقيمة صفرية
                    var defaultWarehouse = await _context.Warehouses.FirstOrDefaultAsync();
                    if (defaultWarehouse != null)
                    {
                        _context.StockLevels.Add(new StockLevel
                        {
                            ProductId = product.Id,
                            WarehouseId = defaultWarehouse.Id,
                            Quantity = 0
                        });
                        await _context.SaveChangesAsync();
                    }

                    // ✅ رسالة للمستخدم بدون تفاصيل تقنية
                    TempData["Success"] = "✅ تم إضافة المنتج بنجاح";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Error"] = "⚠️ البيانات غير صالحة";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"❌ حدث خطأ أثناء حفظ المنتج: {ex.Message}";
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View(product);
        }

        // 🟡 تعديل المنتج
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            ViewBag.Categories = _context.Categories.ToList();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var stockQty = _context.StockLevels
                    .Where(s => s.ProductId == product.Id)
                    .Sum(s => s.Quantity);

                product.Status = stockQty == 0
                    ? "غير متوفر"
                    : product.ReorderLevel.HasValue && stockQty <= product.ReorderLevel
                        ? "منخفض"
                        : "متوفر";

                _context.Update(product);
                await _context.SaveChangesAsync();

                TempData["Success"] = "✅ تم تعديل المنتج بنجاح";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = _context.Categories.ToList();
            TempData["Error"] = "❌ حدث خطأ أثناء تعديل المنتج";
            return View(product);
        }

        // 🟣 عرض تفاصيل المنتج
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.StockLevels)
                    .ThenInclude(sl => sl.Warehouse)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // 🟡 حذف المنتج
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            TempData["Success"] = "🗑️ تم حذف المنتج بنجاح";
            return RedirectToAction(nameof(Index));
        }
    }
}
