using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ERP_System.Models;
using ERP_System.ViewModels;
using ERP_System.Services.Interfaces;

namespace ERP_System.Controllers
{
    [Authorize]
    public class DefineUnitController : Controller
    {
        private readonly IUnitService _unitService;

        public DefineUnitController(IUnitService unitService)
        {
            _unitService = unitService;
        }
        public async Task<IActionResult> List()
        {
            var units = await _unitService.GetAllAsync();
            return View(units);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(AddUnitVm advm)
        {
            if (!ModelState.IsValid) return View(advm);

            var unit = new Unit
            {
                Name = advm.Name,
                Details = advm.Detail
            };

            await _unitService.AddAsync(unit);
            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var unit = await _unitService.GetByIdAsync(id);
            if (unit == null) return NotFound();

            var vm = new AddUnitVm
            {
                Name = unit.Name,
                Detail = unit.Details
            };
            ViewBag.Id = id;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AddUnitVm vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Id = id;
                return View(vm);
            }

            var unit = new Unit
            {
                Id = id,
                Name = vm.Name,
                Details = vm.Detail
            };

            await _unitService.UpdateAsync(unit);
            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Details(int id)
        {
            var unit = await _unitService.GetByIdAsync(id);
            if (unit == null) return NotFound();
            return View(unit);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _unitService.DeleteAsync(id);
            return RedirectToAction(nameof(List));
        }
    }
}
