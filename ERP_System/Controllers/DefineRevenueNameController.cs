using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ERP_System.Models;
using ERP_System.Data;
using ERP_System.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ERP_System.Controllers
{
    [Authorize]
    public class DefineRevenueNameController : Controller
    {
        private readonly AppDbContext _context;

        public DefineRevenueNameController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> List()
        {
            var items = await _context.RevenueNames.ToListAsync();
            return View(items);
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(AddRevenueNameVm advm)
        {
            if (!ModelState.IsValid) return View(advm);

            var rev = new RevenueName
            {
                Name = advm.Name,
                Description = advm.Detail
            };

            _context.RevenueNames.Add(rev);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "تم الحفظ بنجاح";
            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var revenue = await _context.RevenueNames.FindAsync(id);
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

            var revenue = await _context.RevenueNames.FindAsync(id);
            if (revenue == null) return NotFound();

            revenue.Name = vm.Name;
            revenue.Description = vm.Detail;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Details(int id)
        {
            var revenue = await _context.RevenueNames.FindAsync(id);
            if (revenue == null) return NotFound();
            return View(revenue);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var revenue = await _context.RevenueNames.FindAsync(id);
            if (revenue != null)
            {
                _context.RevenueNames.Remove(revenue);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(List));
        }
    }
}
