using ERP_System.Data;
using ERP_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP_System.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly AppDbContext _context;

        public SuppliersController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ عرض جميع الموردين مع تحميل فواتير الشراء
        public async Task<IActionResult> Index()
        {
            var suppliers = await _context.Suppliers
                .Include(s => s.PurchaseInvoices)
                .AsNoTracking()
                .ToListAsync();

            return View(suppliers);
        }

        // ✅ صفحة إنشاء مورد جديد
        public IActionResult Create()
        {
            return View(new Supplier());
        }

        // ✅ تنفيذ عملية الإضافة - مع عرض الأخطاء التفصيلية
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Supplier supplier)
        {
            // إزالة العلاقات من التحقق
            ModelState.Remove("Payments");
            ModelState.Remove("PurchaseInvoices");
            ModelState.Remove("Id");

            if (!ModelState.IsValid)
            {
                var allErrors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new
                    {
                        Field = x.Key,
                        Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                    })
                    .ToList();

                string errorMessages = string.Join("<br>", allErrors.Select(e =>
                    $"<b>{e.Field}</b>: {string.Join(", ", e.Errors)}"));

                TempData["Error"] = $"⚠️ تأكد من إدخال البيانات بشكل صحيح:<br>{errorMessages}";
                return View(supplier);
            }

            try
            {
                _context.Suppliers.Add(supplier);
                await _context.SaveChangesAsync();

                TempData["Success"] = "✅ تم إضافة المورد بنجاح!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"❌ حدث خطأ أثناء حفظ المورد: {ex.Message}";
                return View(supplier);
            }
        }

        // ✅ صفحة تعديل المورد
        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                TempData["Error"] = "⚠️ المورد غير موجود.";
                return RedirectToAction(nameof(Index));
            }

            return View(supplier);
        }

        // ✅ تنفيذ عملية التعديل
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Supplier supplier)
        {
            if (id != supplier.Id)
                return NotFound();

            ModelState.Remove("Payments");
            ModelState.Remove("PurchaseInvoices");

            if (!ModelState.IsValid)
            {
                var allErrors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new
                    {
                        Field = x.Key,
                        Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                    })
                    .ToList();

                string errorMessages = string.Join("<br>", allErrors.Select(e =>
                    $"<b>{e.Field}</b>: {string.Join(", ", e.Errors)}"));

                TempData["Error"] = $"⚠️ تأكد من صحة البيانات:<br>{errorMessages}";
                return View(supplier);
            }

            try
            {
                _context.Entry(supplier).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                TempData["Success"] = "✅ تم تعديل المورد بنجاح!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExists(supplier.Id))
                    return NotFound();

                TempData["Error"] = "⚠️ حدث خطأ أثناء تعديل المورد، حاول مرة أخرى.";
                return View(supplier);
            }
        }

        // ✅ عرض تفاصيل المورد
        public async Task<IActionResult> Details(int id)
        {
            var supplier = await _context.Suppliers
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (supplier == null)
            {
                TempData["Error"] = "⚠️ المورد غير موجود.";
                return RedirectToAction(nameof(Index));
            }

            return View(supplier);
        }

        // ✅ صفحة تأكيد الحذف
        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await _context.Suppliers
                .Include(s => s.PurchaseInvoices)
                .Include(s => s.Payments)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (supplier == null)
            {
                TempData["Error"] = "⚠️ المورد غير موجود.";
                return RedirectToAction(nameof(Index));
            }

            return View(supplier);
        }

        // ✅ تنفيذ عملية الحذف (مع التحقق من العلاقات)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supplier = await _context.Suppliers
                .Include(s => s.PurchaseInvoices)
                .Include(s => s.Payments)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (supplier == null)
            {
                TempData["Error"] = "⚠️ المورد غير موجود.";
                return RedirectToAction(nameof(Index));
            }

            // التحقق من وجود علاقات تمنع الحذف
            if ((supplier.PurchaseInvoices?.Any() ?? false) || (supplier.Payments?.Any() ?? false))
            {
                TempData["Error"] = "⚠️ لا يمكن حذف المورد لأنه مرتبط بفواتير أو مدفوعات.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Suppliers.Remove(supplier);
                await _context.SaveChangesAsync();

                TempData["Success"] = "🗑️ تم حذف المورد بنجاح!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"❌ فشل حذف المورد: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // ✅ التحقق من وجود المورد
        private bool SupplierExists(int id)
        {
            return _context.Suppliers.Any(e => e.Id == id);
        }
    }
}
