using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP_System.Models;
using ERP_System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public async Task<IActionResult> List()
        {
            var transfers = await _context.InvoiceTransferHeaders
                .Include(t => t.FromStore)
                .Include(t => t.ToStore)
                .Include(t => t.AssignedByEmployee)
                .OrderByDescending(t => t.Date)
                .ToListAsync();
            return View(transfers);
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(List));
        }

        public IActionResult Create()
        {
            ViewBag.Stores = new SelectList(_context.Stores, "Id", "Name");
            ViewBag.Items = _context.Items.Select(i => new { i.Id, i.Name, i.SalePrice }).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InvoiceTransferHeader transfer, List<int> ItemIds, List<int> Quantities)
        {
            if (ModelState.IsValid)
            {
                // Generate Code
                var lastTransfer = await _context.InvoiceTransferHeaders.OrderByDescending(t => t.Id).FirstOrDefaultAsync();
                int nextId = (lastTransfer?.Id ?? 0) + 1;
                transfer.Code = $"TRF-{nextId:D6}";
                transfer.Date = DateTime.Now;

                // Get current user employee id
                var username = User.Identity.Name;
                var login = await _context.Logins.FirstOrDefaultAsync(l => l.Username == username);
                if (login != null)
                {
                    transfer.AssignedBy = login.EmpId;
                }

                _context.Add(transfer);
                await _context.SaveChangesAsync();

                // Add Details
                if (ItemIds != null && Quantities != null)
                {
                    for (int i = 0; i < ItemIds.Count; i++)
                    {
                        if (ItemIds[i] != 0 && Quantities[i] > 0)
                        {
                            var detail = new InvoiceTransferDetail
                            {
                                HeaderId = transfer.Id,
                                ItemId = ItemIds[i],
                                Quantity = Quantities[i]
                            };
                            _context.Add(detail);
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(List));
            }
            ViewBag.Stores = new SelectList(_context.Stores, "Id", "Name");
            ViewBag.Items = _context.Items.Select(i => new { i.Id, i.Name, i.SalePrice }).ToList();
            return View(transfer);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var transfer = await _context.InvoiceTransferHeaders
                .Include(t => t.Details)
                .ThenInclude(d => d.Item)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (transfer == null) return NotFound();

            ViewBag.Stores = new SelectList(_context.Stores, "Id", "Name");
            ViewBag.Items = _context.Items.Select(i => new { i.Id, i.Name, i.SalePrice }).ToList();
            return View(transfer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, InvoiceTransferHeader transfer, List<int> ItemIds, List<int> Quantities)
        {
            if (id != transfer.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingTransfer = await _context.InvoiceTransferHeaders
                        .Include(t => t.Details)
                        .FirstOrDefaultAsync(t => t.Id == id);

                    if (existingTransfer == null) return NotFound();

                    existingTransfer.FromStoreId = transfer.FromStoreId;
                    existingTransfer.ToStoreId = transfer.ToStoreId;
                    existingTransfer.Remarks = transfer.Remarks;
                    // Date and Code usually shouldn't change on edit, but depends on requirements. Keeping them as is.

                    // Update Details
                    _context.InvoiceTransferDetails.RemoveRange(existingTransfer.Details);

                    if (ItemIds != null && Quantities != null)
                    {
                        for (int i = 0; i < ItemIds.Count; i++)
                        {
                            if (ItemIds[i] != 0 && Quantities[i] > 0)
                            {
                                var detail = new InvoiceTransferDetail
                                {
                                    HeaderId = existingTransfer.Id,
                                    ItemId = ItemIds[i],
                                    Quantity = Quantities[i]
                                };
                                _context.Add(detail);
                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceTransferHeaderExists(transfer.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(List));
            }
            ViewBag.Stores = new SelectList(_context.Stores, "Id", "Name");
            ViewBag.Items = _context.Items.Select(i => new { i.Id, i.Name, i.SalePrice }).ToList();
            return View(transfer);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var transfer = await _context.InvoiceTransferHeaders
                .Include(t => t.FromStore)
                .Include(t => t.ToStore)
                .Include(t => t.AssignedByEmployee)
                .Include(t => t.Details)
                .ThenInclude(d => d.Item)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (transfer == null) return NotFound();

            return View(transfer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transfer = await _context.InvoiceTransferHeaders.FindAsync(id);
            if (transfer != null)
            {
                _context.InvoiceTransferHeaders.Remove(transfer);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(List));
        }

        private bool InvoiceTransferHeaderExists(int id)
        {
            return _context.InvoiceTransferHeaders.Any(e => e.Id == id);
        }
    }
}
