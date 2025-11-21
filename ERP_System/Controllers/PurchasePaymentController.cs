using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP_System.Models;
using ERP_System.Data;

namespace ERP_System.Controllers
{
    public class PurchasePaymentController : Controller
    {
        private readonly AppDbContext _context;

        public PurchasePaymentController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> List()
        {
            var payments = await _context.PurchasePayments
                .Include(p => p.InvoicePurchase)
                .ThenInclude(i => i.Supplier)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
            return View(payments);
        }

        public IActionResult Index()
        {
            ViewBag.Invoices = _context.InvoicePurchaseHeaders
                .Include(i => i.Supplier)
                .Where(i => i.PayStatus != "Paid")
                .Select(i => new { i.Id, Text = $"#{i.Id} - {i.Supplier.Name} ({i.Remain})" })
                .ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(PurchasePayment payment)
        {
            if (ModelState.IsValid)
            {
                _context.PurchasePayments.Add(payment);

                // Update Invoice Status
                var invoice = await _context.InvoicePurchaseHeaders.FindAsync(payment.InvPurchaseId);
                if (invoice != null)
                {
                    invoice.Paid += payment.AmountPaid;
                    invoice.Remain = invoice.NetAmount - invoice.Paid;
                    if (invoice.Remain <= 0) invoice.PayStatus = "Paid";
                    else invoice.PayStatus = "Partial";
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }
            ViewBag.Invoices = _context.InvoicePurchaseHeaders
                .Include(i => i.Supplier)
                .Where(i => i.PayStatus != "Paid")
                .Select(i => new { i.Id, Text = $"#{i.Id} - {i.Supplier.Name} ({i.Remain})" })
                .ToList();
            return View(payment);
        }

        public async Task<IActionResult> Details(int id)
        {
            var payment = await _context.PurchasePayments
                .Include(p => p.InvoicePurchase)
                .ThenInclude(i => i.Supplier)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (payment == null) return NotFound();

            return View(payment);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var payment = await _context.PurchasePayments.FindAsync(id);
            if (payment != null)
            {
                // Revert Invoice Status
                var invoice = await _context.InvoicePurchaseHeaders.FindAsync(payment.InvPurchaseId);
                if (invoice != null)
                {
                    invoice.Paid -= payment.AmountPaid;
                    invoice.Remain = invoice.NetAmount - invoice.Paid;
                    if (invoice.Paid == 0) invoice.PayStatus = "Unpaid";
                    else invoice.PayStatus = "Partial";
                }

                _context.PurchasePayments.Remove(payment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(List));
        }
    }
}
