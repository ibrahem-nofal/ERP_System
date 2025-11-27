using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ERP_System.Models;
using ERP_System.Data;
using ERP_System.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ERP_System.Controllers
{
    [Authorize]
    public class DefineExpenseNameController : Controller
    {
        private readonly AppDbContext _context;

        public DefineExpenseNameController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> List()
        {
            var items = await _context.ExpenseNames.ToListAsync();
            return View(items);
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(AddExpenseNameVm advm)
        {
            if (!ModelState.IsValid) return View(advm);

            var exp = new ExpenseName
            {
                Name = advm.Name,
                Description = advm.Detail
            };

            _context.ExpenseNames.Add(exp);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "تم الحفظ بنجاح";
            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var expense = await _context.ExpenseNames.FindAsync(id);
            if (expense == null) return NotFound();

            var vm = new AddExpenseNameVm
            {
                Name = expense.Name,
                Detail = expense.Description
            };
            ViewBag.Id = id;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AddExpenseNameVm vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Id = id;
                return View(vm);
            }

            var expense = await _context.ExpenseNames.FindAsync(id);
            if (expense == null) return NotFound();

            expense.Name = vm.Name;
            expense.Description = vm.Detail;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Details(int id)
        {
            var expense = await _context.ExpenseNames.FindAsync(id);
            if (expense == null) return NotFound();
            return View(expense);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var expense = await _context.ExpenseNames.FindAsync(id);
            if (expense != null)
            {
                _context.ExpenseNames.Remove(expense);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(List));
        }
    }
}
