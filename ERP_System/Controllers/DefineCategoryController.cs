using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ERP_System.Models;
using ERP_System.Data;
using ERP_System.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ERP_System.Controllers
{
    [Authorize]
    public class DefineCategoryController : Controller
    {
        private readonly AppDbContext _context;

        public DefineCategoryController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> List()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(AddCategoryVm advm)
        {
            if (!ModelState.IsValid) return View(advm);

            var cat = new Category
            {
                Name = advm.Name,
                Detail = advm.Detail
            };

            _context.Categories.Add(cat);
            _context.SaveChanges();
            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            var vm = new AddCategoryVm
            {
                Name = category.Name,
                Detail = category.Detail
            };
            ViewBag.Id = id;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AddCategoryVm vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Id = id;
                return View(vm);
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            category.Name = vm.Name;
            category.Detail = vm.Detail;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Details(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(List));
        }
    }
}
