using Microsoft.AspNetCore.Mvc;
using ERP_System.Models;
using ERP_System.Data;
using ERP_System.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ERP_System.Controllers
{
    public class DefineUnitController : Controller
    {
        private readonly AppDbContext _context;

        public DefineUnitController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> List()
        {
            var units = await _context.Units.ToListAsync();
            return View(units);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(AddUnitVm advm)
        {
            if (!ModelState.IsValid) return View(advm);

            var unit = new Unit
            {
                Name = advm.Name,
                Details = advm.Detail
            };

            _context.Units.Add(unit);
            _context.SaveChanges();
            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var unit = await _context.Units.FindAsync(id);
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

            var unit = await _context.Units.FindAsync(id);
            if (unit == null) return NotFound();

            unit.Name = vm.Name;
            unit.Details = vm.Detail;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Details(int id)
        {
            var unit = await _context.Units.FindAsync(id);
            if (unit == null) return NotFound();
            return View(unit);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var unit = await _context.Units.FindAsync(id);
            if (unit != null)
            {
                _context.Units.Remove(unit);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(List));
        }
    }
}
