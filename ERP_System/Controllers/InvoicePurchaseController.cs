using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP_System.Models;
using ERP_System.Data;

namespace ERP_System.Controllers
{
    [Authorize]
    public class InvoicePurchaseController : Controller
    {
        private readonly AppDbContext _context;

        public InvoicePurchaseController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> List()
        {
            var invoices = await _context.InvoicePurchaseHeaders
                .Include(i => i.Supplier)
                .OrderByDescending(i => i.DateCreated)
                .ToListAsync();
            return View(invoices);
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.InvoicePurchaseHeaders
                .Include(i => i.Supplier)
                .Include(i => i.Store)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (invoice == null)
            {
                return NotFound();
            }

            var details = await _context.InvoicePurchaseDetails
                .Include(d => d.Item)
                .Where(d => d.InvoiceId == id)
                .ToListAsync();

            ViewBag.Details = details;

            return View(invoice);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.InvoicePurchaseHeaders.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            return View(invoice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SupplierId,StoreId,Remarks,PayStatus,OrderStatus,DeliveryDate,IsPostpaid,PaymentDueDate")] InvoicePurchaseHeader invoice)
        {
            if (id != invoice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingInvoice = await _context.InvoicePurchaseHeaders.FindAsync(id);
                    if (existingInvoice == null)
                    {
                        return NotFound();
                    }

                    // Update only allowed fields
                    existingInvoice.Remarks = invoice.Remarks;

                    // Map PayStatus to database allowed values (open/closed)
                    existingInvoice.PayStatus = invoice.PayStatus == "Paid" ? "closed" : "open";
                    existingInvoice.OrderStatus = invoice.OrderStatus;
                    existingInvoice.DeliveryDate = invoice.DeliveryDate;
                    existingInvoice.IsPostpaid = invoice.IsPostpaid;
                    existingInvoice.PaymentDueDate = invoice.PaymentDueDate;

                    _context.Update(existingInvoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(List));
            }
            return View(invoice);
        }

        public IActionResult Create()
        {
            ViewData["SupplierId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Suppliers, "Id", "Name");
            ViewData["StoreId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Stores, "Id", "Name");
            ViewData["Items"] = _context.Items.Select(i => new { i.Id, i.Name, i.BuyPrice }).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SupplierId,StoreId,DateCreated,DeliveryDate,PayStatus,OrderStatus,Remarks,IsPostpaid,PaymentDueDate,Discount")] InvoicePurchaseHeader invoice, List<InvoicePurchaseDetail> details)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Set default InvType based on original PayStatus
                    invoice.InvType = invoice.PayStatus == "Paid" ? "PurchaseCash" : "PurchaseCredit";

                    // Map PayStatus to database allowed values (open/closed)
                    invoice.PayStatus = invoice.PayStatus == "Paid" ? "closed" : "open";

                    // 1. Save Header first to get Id
                    _context.Add(invoice);
                    await _context.SaveChangesAsync();

                    // 2. Process Details
                    decimal totalAmount = 0;
                    if (details != null && details.Count > 0)
                    {
                        foreach (var item in details)
                        {
                            item.InvoiceId = invoice.Id;
                            item.Status = "Purchased"; // Set default status

                            // Calculate TotalPrice for the item
                            totalAmount += item.Quantity * item.UnitPrice;

                            _context.Add(item);
                        }
                        await _context.SaveChangesAsync();
                    }

                    // 3. Update Header TotalAmount
                    invoice.TotalAmount = totalAmount;

                    // Calculate Paid and Remain based on PayStatus
                    // Note: NetAmount is computed in DB, so we calculate effective net amount here for logic
                    decimal effectiveNetAmount = totalAmount - invoice.Discount;

                    if (invoice.PayStatus == "closed") // Paid
                    {
                        invoice.Paid = effectiveNetAmount;
                        invoice.Remain = 0;
                    }
                    else // Open (Unpaid or Partial)
                    {
                        invoice.Paid = 0;
                        invoice.Remain = effectiveNetAmount;
                    }

                    _context.Update(invoice);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(List));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error saving invoice: " + ex.Message + (ex.InnerException != null ? " Inner: " + ex.InnerException.Message : ""));
                }
            }

            ViewData["SupplierId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Suppliers, "Id", "Name", invoice.SupplierId);
            ViewData["StoreId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Stores, "Id", "Name", invoice.StoreId);
            ViewData["Items"] = _context.Items.Select(i => new { i.Id, i.Name, i.BuyPrice }).ToList();
            return View(invoice);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _context.InvoicePurchaseHeaders.FindAsync(id);
            if (invoice != null)
            {
                _context.InvoicePurchaseHeaders.Remove(invoice);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(List));
        }

        private bool InvoiceExists(int id)
        {
            return _context.InvoicePurchaseHeaders.Any(e => e.Id == id);
        }
    }
}
