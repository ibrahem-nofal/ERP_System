using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP_System.Models;
using ERP_System.Data;

namespace ERP_System.Controllers
{
    [Authorize]
    public class ChartOfAccountController : Controller
    {
        private readonly AppDbContext _context;

        public ChartOfAccountController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> List()
        {
            var accounts = await _context.ChartOfAccounts.OrderBy(a => a.Code).ToListAsync();
            return View(accounts);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ChartOfAccount account)
        {
            if (ModelState.IsValid)
            {
                _context.ChartOfAccounts.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }
            return View(account);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var account = await _context.ChartOfAccounts.FindAsync(id);
            if (account == null) return NotFound();
            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ChartOfAccount account)
        {
            if (id != account.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }
            return View(account);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var account = await _context.ChartOfAccounts.FindAsync(id);
            if (account != null)
            {
                _context.ChartOfAccounts.Remove(account);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(List));
        }
    }
}
