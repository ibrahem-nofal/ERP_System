using ERP_System.Data;
using ERP_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP_System.Controllers
{
    public class StockTransfersController : Controller
    {
        private readonly AppDbContext _context;

        public StockTransfersController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ صفحة إنشاء تحويل جديد
        public IActionResult Create()
        {
            ViewBag.Warehouses = _context.Warehouses.ToList();
            ViewBag.Products = _context.Products.Include(p => p.Category).ToList();
            return View();
        }



        // ✅ تنفيذ عملية التحويل
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StockTransfer transfer, int[] productIds, int[] quantities)
        {
            if (transfer.FromWarehouseId == transfer.ToWarehouseId)
            {
                TempData["Error"] = "⚠️ لا يمكن التحويل من وإلى نفس المخزن.";
                ViewBag.Warehouses = _context.Warehouses.ToList();
                ViewBag.Products = _context.Products.ToList();
                return View(transfer);
            }

            if (productIds.Length == 0)
            {
                TempData["Error"] = "⚠️ يجب اختيار منتج واحد على الأقل.";
                ViewBag.Warehouses = _context.Warehouses.ToList();
                ViewBag.Products = _context.Products.ToList();
                return View(transfer);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 🟩 حفظ التحويل الرئيسي
                transfer.TransferDate = DateTime.Now;
                transfer.Status = "تم التحويل";
                _context.StockTransfers.Add(transfer);
                await _context.SaveChangesAsync();

                // 🟩 لكل منتج يتم اختياره
                for (int i = 0; i < productIds.Length; i++)
                {
                    int productId = productIds[i];
                    int qty = quantities[i];

                    if (qty <= 0) continue;

                    // 🟥 خصم من المخزن المصدر
                    var sourceLevel = await _context.StockLevels
                        .FirstOrDefaultAsync(s => s.ProductId == productId && s.WarehouseId == transfer.FromWarehouseId);

                    if (sourceLevel == null || sourceLevel.Quantity < qty)
                    {
                        TempData["Error"] = $"⚠️ الكمية غير متوفرة للمنتج رقم {productId}.";
                        await transaction.RollbackAsync();
                        return RedirectToAction(nameof(Create));
                    }

                    sourceLevel.Quantity -= qty;

                    // 🟩 إضافة للمخزن الوجهة
                    var destLevel = await _context.StockLevels
                        .FirstOrDefaultAsync(s => s.ProductId == productId && s.WarehouseId == transfer.ToWarehouseId);

                    if (destLevel == null)
                    {
                        destLevel = new StockLevel
                        {
                            ProductId = productId,
                            WarehouseId = transfer.ToWarehouseId,
                            Quantity = 0
                        };
                        _context.StockLevels.Add(destLevel);
                    }

                    destLevel.Quantity += qty;

                    // 🟨 إنشاء سجل في جدول StockTransferItem
                    var transferItem = new StockTransferItem
                    {
                        TransferId = transfer.Id,
                        ProductId = productId,
                        Quantity = qty
                    };
                    _context.StockTransferItems.Add(transferItem);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["Success"] = "✅ تم تنفيذ التحويل بنجاح.";
                return RedirectToAction("Index", "Warehouses");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                TempData["Error"] = $"❌ حدث خطأ أثناء التحويل: {ex.Message}";
                return View(transfer);
            }
        }
        // ✅ عرض جميع التحويلات
        public async Task<IActionResult> Index()
        {
            var transfers = await _context.StockTransfers
                .Include(t => t.FromWarehouse)
                .Include(t => t.ToWarehouse)
                .Include(t => t.Items)
                .ThenInclude(i => i.Product)
                .OrderByDescending(t => t.TransferDate)
                .ToListAsync();

            return View(transfers);
        }
        // ✅ تفاصيل تحويل معين
        public async Task<IActionResult> Details(int id)
        {
            var transfer = await _context.StockTransfers
                .Include(t => t.FromWarehouse)
                .Include(t => t.ToWarehouse)
                .Include(t => t.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transfer == null)
                return NotFound();

            return View(transfer);
        }
        // ✅ تأكيد التحويل (خصم الكميات من المخزن المصدر + إضافتها للوجهة)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmTransfer(int id)
        {
            var transfer = await _context.StockTransfers
                .Include(t => t.Items)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transfer == null)
            {
                TempData["Error"] = "❌ لم يتم العثور على التحويل.";
                return RedirectToAction(nameof(Index));
            }

            if (transfer.Status == "تم التحويل")
            {
                TempData["Info"] = "ℹ️ هذا التحويل تم تأكيده بالفعل.";
                return RedirectToAction(nameof(Index));
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var item in transfer.Items)
                {
                    // 🟥 خصم الكمية من المخزن المصدر
                    var source = await _context.StockLevels
                        .FirstOrDefaultAsync(s => s.ProductId == item.ProductId && s.WarehouseId == transfer.FromWarehouseId);

                    if (source == null || source.Quantity < item.Quantity)
                    {
                        TempData["Error"] = $"⚠️ الكمية غير متوفرة للمنتج رقم {item.ProductId}.";
                        await transaction.RollbackAsync();
                        return RedirectToAction(nameof(Index));
                    }

                    source.Quantity -= item.Quantity;

                    // 🟩 إضافة الكمية للمخزن الوجهة
                    var dest = await _context.StockLevels
                        .FirstOrDefaultAsync(s => s.ProductId == item.ProductId && s.WarehouseId == transfer.ToWarehouseId);

                    if (dest == null)
                    {
                        dest = new StockLevel
                        {
                            ProductId = item.ProductId,
                            WarehouseId = transfer.ToWarehouseId,
                            Quantity = 0
                        };
                        _context.StockLevels.Add(dest);
                    }

                    dest.Quantity += item.Quantity;
                }

                // ✅ تحديث حالة التحويل
                transfer.Status = "تم التحويل";
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["Success"] = "✅ تم تأكيد التحويل بنجاح وتحديث الكميات.";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                TempData["Error"] = $"❌ خطأ أثناء تنفيذ التحويل: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }


        // ✅ جلب المنتجات داخل مخزن معين (للاستخدام في AJAX)
        [HttpGet]
        public async Task<IActionResult> GetProductsByWarehouse(int warehouseId)
        {
            var products = await _context.StockLevels
                .Include(s => s.Product)
                .Where(s => s.WarehouseId == warehouseId && s.Quantity > 0)
                .Select(s => new
                {
                    id = s.Product.Id,
                    name = s.Product.Name,
                    quantity = s.Quantity
                })
                .ToListAsync();

            return Json(products);
        }



    }


}
