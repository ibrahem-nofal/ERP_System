using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP_System.Models;
using ERP_System.Data;
using ERP_System.ViewModels;

namespace ERP_System.Controllers
{
    [Authorize]
    public class JournalEntryController : Controller
    {
        private readonly AppDbContext _context;

        public JournalEntryController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> List()
        {
            var entries = await _context.JournalEntries
                .Include(j => j.AssignedByEmployee)
                .OrderByDescending(j => j.CreatedAt)
                .ToListAsync();
            return View(entries);
        }

        public IActionResult Index()
        {
            ViewBag.Accounts = _context.ChartOfAccounts.Where(a => a.IsLeaf).ToList();
            return View(new JournalEntryVm());
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] JournalEntryVm vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (vm.Details.Sum(d => d.Debit) != vm.Details.Sum(d => d.Credit))
            {
                return BadRequest("Total Debit must equal Total Credit.");
            }

            var entry = new JournalEntry
            {
                Description = vm.Description,
                CreatedAt = vm.Date,
                SourceType = "Manual",
                // AssignedBy = User.Identity.GetUserId() // Implement auth later
                Details = vm.Details.Select(d => new JournalDetail
                {
                    AccountId = d.AccountId,
                    Debit = d.Debit,
                    Credit = d.Credit,
                    Note = d.Note
                }).ToList()
            };

            _context.JournalEntries.Add(entry);
            await _context.SaveChangesAsync();

            return Ok(new { id = entry.Id });
        }

        public async Task<IActionResult> Details(int id)
        {
            var entry = await _context.JournalEntries
                .Include(j => j.Details)
                .ThenInclude(d => d.Account)
                .Include(j => j.AssignedByEmployee)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (entry == null) return NotFound();

            return View(entry);
        }
    }
}
