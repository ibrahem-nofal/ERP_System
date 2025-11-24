using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP_System.Models;
using ERP_System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ERP_System.Controllers
{
    [Authorize]
    public class DefineDelegateMemberController : Controller
    {
        private readonly AppDbContext _context;

        public DefineDelegateMemberController(AppDbContext context)
        {
            _context = context;
        }

        // List all delegate members
        public async Task<IActionResult> Index()
        {
            var delegates = await _context.DelegateMembers
                .Include(d => d.Employee)
                .ToListAsync();
            return View(delegates);
        }

        // Create new delegate member (GET)
        public IActionResult Create()
        {
            // Get employees who are not already delegates
            var existingDelegateIds = _context.DelegateMembers.Select(d => d.EmpId).ToList();
            var employees = _context.Employees
                .Where(e => !existingDelegateIds.Contains(e.Id))
                .Select(e => new { e.Id, e.Name })
                .ToList();

            ViewBag.Employees = new SelectList(employees, "Id", "Name");
            return View();
        }

        // Create new delegate member (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DelegateMember delegateMember)
        {
            if (ModelState.IsValid)
            {
                // Check if already exists (double check)
                if (_context.DelegateMembers.Any(d => d.EmpId == delegateMember.EmpId))
                {
                    ModelState.AddModelError("", "This employee is already a delegate member.");
                }
                else
                {
                    _context.Add(delegateMember);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            // Reload list if failed
            var existingDelegateIds = _context.DelegateMembers.Select(d => d.EmpId).ToList();
            var employees = _context.Employees
                .Where(e => !existingDelegateIds.Contains(e.Id))
                .Select(e => new { e.Id, e.Name })
                .ToList();
            ViewBag.Employees = new SelectList(employees, "Id", "Name", delegateMember.EmpId);

            return View(delegateMember);
        }

        // Delete delegate member (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var delegateMember = await _context.DelegateMembers
                .Include(d => d.Employee)
                .FirstOrDefaultAsync(m => m.EmpId == id);
            if (delegateMember == null)
            {
                return NotFound();
            }

            return View(delegateMember);
        }

        // Delete delegate member (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var delegateMember = await _context.DelegateMembers.FindAsync(id);
            if (delegateMember != null)
            {
                _context.DelegateMembers.Remove(delegateMember);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
