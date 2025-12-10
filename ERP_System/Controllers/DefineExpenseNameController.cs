using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ERP_System.Models;
using ERP_System.ViewModels;
using ERP_System.Services.Interfaces;

namespace ERP_System.Controllers
{
    [Authorize]
    public class DefineExpenseNameController : Controller
    {
        private readonly IExpenseNameService _expenseNameService;

        public DefineExpenseNameController(IExpenseNameService expenseNameService)
        {
            _expenseNameService = expenseNameService;
        }

        public async Task<IActionResult> List()
        {
            var items = await _expenseNameService.GetAllAsync();
            return View(items);
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(AddExpenseNameVm advm)
        {
            if (!ModelState.IsValid) return View(advm);

            var exp = new ExpenseName
            {
                Name = advm.Name,
                Description = advm.Detail
            };

            await _expenseNameService.AddAsync(exp);
            TempData["SuccessMessage"] = "تم الحفظ بنجاح";
            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var expense = await _expenseNameService.GetByIdAsync(id);
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

            var expense = new ExpenseName
            {
                Id = id,
                Name = vm.Name,
                Description = vm.Detail
            };

            await _expenseNameService.UpdateAsync(expense);
            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Details(int id)
        {
            var expense = await _expenseNameService.GetByIdAsync(id);
            if (expense == null) return NotFound();
            return View(expense);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _expenseNameService.DeleteAsync(id);
            return RedirectToAction(nameof(List));
        }
    }
}
