using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP_System.Models;
using ERP_System.Data;
using ERP_System.Services.Interfaces;
using ERP_System.ViewModels;

namespace ERP_System.Controllers
{
    [Authorize]
    public class InvoiceSaleController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IInventoryService _inventoryService;
        private readonly IJournalEntryService _journalEntryService;

        public InvoiceSaleController(AppDbContext context, IInventoryService inventoryService, IJournalEntryService journalEntryService)
        {
            _context = context;
            _inventoryService = inventoryService;
            _journalEntryService = journalEntryService;
        }

        public async Task<IActionResult> List()
        {
            var invoices = await _context.InvoiceSaleHeaders
                .Include(i => i.Customer)
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

            var invoice = await _context.InvoiceSaleHeaders
                .Include(i => i.Customer)
                .Include(i => i.Store)
                .Include(i => i.Delegate)
                    .ThenInclude(d => d.Employee)
                .Include(i => i.AssignedByEmployee)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (invoice == null)
            {
                return NotFound();
            }

            // Load details separately to avoid potential cartesian explosion if multiple collection includes
            var details = await _context.InvoiceSaleDetails
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

            var invoice = await _context.InvoiceSaleHeaders.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            return View(invoice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Remarks,PayStatus,OrderStatus,DeliveryDate,IsPostpaid,PaymentDueDate")] InvoiceSaleHeader invoice)
        {
            if (id != invoice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingInvoice = await _context.InvoiceSaleHeaders.FindAsync(id);
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
            ViewData["CustomerId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Customers, "Id", "Name");
            ViewData["StoreId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Stores, "Id", "Name");
            ViewData["DelegateId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.DelegateMembers.Include(d => d.Employee).Select(d => new { Id = d.EmpId, Name = d.Employee.Name }), "Id", "Name");
            ViewData["Items"] = _context.Items.Select(i => new { i.Id, i.Name, i.SalePrice }).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerId,StoreId,DelegateId,DateCreated,DeliveryDate,PayStatus,OrderStatus,Remarks,IsPostpaid,PaymentDueDate,Discount")] InvoiceSaleHeader invoice, List<InvoiceSaleDetail> details)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Set default InvType based on original PayStatus
                    invoice.InvType = invoice.PayStatus == "Paid" ? "SalesCash" : "SalesCredit";

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
                            
                            totalAmount += item.Quantity * item.UnitPrice;

                            _context.Add(item);

                            // Update Inventory
                            if (invoice.StoreId.HasValue)
                            {
                                await _inventoryService.RemoveStockAsync(
                                   item.ItemId,
                                   invoice.StoreId.Value,
                                   item.Quantity,
                                   $"فاتورة بيع #{invoice.Id}",
                                   invoice.Id,
                                   "Sale"
                               );
                            }
                        }
                        await _context.SaveChangesAsync();
                    }

                    // 3. Update Header TotalAmount
                    invoice.TotalAmount = totalAmount;

                    // 4. Create Automatic Journal Entry
                    await CreateSaleJournalEntry(invoice, details);

                    _context.Update(invoice);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(List));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error saving invoice: " + ex.Message + (ex.InnerException != null ? " Inner: " + ex.InnerException.Message : ""));
                }
            }

            ViewData["CustomerId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Customers, "Id", "Name", invoice.CustomerId);
            ViewData["StoreId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Stores, "Id", "Name", invoice.StoreId);
            ViewData["DelegateId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.DelegateMembers.Include(d => d.Employee).Select(d => new { Id = d.EmpId, Name = d.Employee.Name }), "Id", "Name", invoice.DelegateId);
            ViewData["Items"] = _context.Items.Select(i => new { i.Id, i.Name, i.SalePrice }).ToList();
            return View(invoice);
        }

        private async Task CreateSaleJournalEntry(InvoiceSaleHeader invoice, List<InvoiceSaleDetail> details)
        {
            // 1. Get Accounts
            int cashAccId = await _journalEntryService.GetOrCreateAccountAsync("النقدية", "1100", "Asset");
            int arAccId = await _journalEntryService.GetOrCreateAccountAsync("العملاء", "1101", "Asset");
            int salesAccId = await _journalEntryService.GetOrCreateAccountAsync("المبيعات", "4100", "Revenue");
            int cogsAccId = await _journalEntryService.GetOrCreateAccountAsync("تكلفة البضاعة المباعة", "5100", "Expense");
            int inventoryAccId = await _journalEntryService.GetOrCreateAccountAsync("المخزون", "1200", "Asset");

            var journalDetails = new List<JournalDetail>();

            // --- Part A: Revenue (Sales) ---
            // Debit: Cash or AR
            if (invoice.PayStatus == "closed")
            {
                journalDetails.Add(new JournalDetail { AccountId = cashAccId, Debit = invoice.TotalAmount, Credit = 0, Note = "تحصيل نقدي - مبيعات" });
            }
            else
            {
                journalDetails.Add(new JournalDetail { AccountId = arAccId, Debit = invoice.TotalAmount, Credit = 0, Note = $"استحقاق على العميل {invoice.Customer?.Name}" });
            }

            // Credit: Sales Revenue
            journalDetails.Add(new JournalDetail { AccountId = salesAccId, Debit = 0, Credit = invoice.TotalAmount, Note = $"مبيعات فاتورة #{invoice.Id}" });


            // --- Part B: Cost of Goods Sold (COGS) ---
            // We need to fetch BuyPrice (Cost) for the items
            var itemIds = details.Select(d => d.ItemId).Distinct().ToList();
            var items = await _context.Items.Where(i => itemIds.Contains(i.Id)).ToDictionaryAsync(i => i.Id);

            decimal totalCost = 0;
            foreach (var d in details)
            {
                if (items.TryGetValue(d.ItemId, out var item))
                {
                    decimal cost = (item.BuyPrice ?? 0) * d.Quantity;
                    totalCost += cost;
                }
            }

            if (totalCost > 0)
            {
                // Debit: COGS
                journalDetails.Add(new JournalDetail { AccountId = cogsAccId, Debit = totalCost, Credit = 0, Note = "ت. البضاعة المباعة" });
                // Credit: Inventory
                journalDetails.Add(new JournalDetail { AccountId = inventoryAccId, Debit = 0, Credit = totalCost, Note = "صرف من المخزون" });
            }


            // 3. Create Entry
            var entry = new JournalEntry
            {
                Description = $"قيد مبيعات - فاتورة #{invoice.Id} - {invoice.Customer?.Name}",
                CreatedAt = invoice.DateCreated,
                SourceType = "Sale Invoice",
                InvSaleId = invoice.Id,
                Details = journalDetails
            };

            await _journalEntryService.CreateAutomaticEntryAsync(entry);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _context.InvoiceSaleHeaders.FindAsync(id);
            if (invoice != null)
            {
                _context.InvoiceSaleHeaders.Remove(invoice);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(List));
        }

        private bool InvoiceExists(int id)
        {
            return _context.InvoiceSaleHeaders.Any(e => e.Id == id);
        }
    }
}
