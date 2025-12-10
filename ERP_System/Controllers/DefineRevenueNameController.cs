using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ERP_System.Models;
using ERP_System.ViewModels;
using ERP_System.Services.Interfaces;

namespace ERP_System.Controllers
{
    [Authorize]
    public class DefineRevenueNameController : Controller
    {
        private readonly IRevenueNameService _revenueNameService;

        public DefineRevenueNameController(IRevenueNameService revenueNameService)
        {
            _revenueNameService = revenueNameService;
        }

        public async Task<IActionResult> List()
        {
            var items = await _revenueNameService.GetAllAsync();
            return View(items);
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(AddRevenueNameVm advm)
        {
            if (!ModelState.IsValid) return View(advm);

            var rev = new RevenueName
            {
                Name = advm.Name,
                Description = advm.Detail
            };

            await _revenueNameService.AddAsync(rev);
            TempData["SuccessMessage"] = "تم الحفظ بنجاح";
            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var revenue = await _revenueNameService.GetByIdAsync(id);
            if (revenue == null) return NotFound();

            var vm = new AddRevenueNameVm
            {
                Name = revenue.Name,
                Detail = revenue.Description
            };
            ViewBag.Id = id;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AddRevenueNameVm vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Id = id;
                return View(vm);
            }

            var revenue = new RevenueName
            {
                Id = id,
                Name = vm.Name,
                Description = vm.Detail
            };

            await _revenueNameService.UpdateAsync(revenue);
            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Details(int id)
        {
            var revenue = await _revenueNameService.GetByIdAsync(id);
            if (revenue == null) return NotFound();
            return View(revenue);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _revenueNameService.DeleteAsync(id);
            return RedirectToAction(nameof(List));
        }
    }
}
