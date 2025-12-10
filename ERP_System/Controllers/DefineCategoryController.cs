using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ERP_System.Models;
using ERP_System.ViewModels;
using ERP_System.Services.Interfaces;

namespace ERP_System.Controllers
{
    [Authorize]
    public class DefineCategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public DefineCategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> List()
        {
            var categories = await _categoryService.GetAllAsync();
            return View(categories);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(AddCategoryVm advm)
        {
            if (!ModelState.IsValid) return View(advm);

            var cat = new Category
            {
                Name = advm.Name,
                Detail = advm.Detail
            };

            await _categoryService.AddAsync(cat);
            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
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

            var category = new Category
            {
                Id = id,
                Name = vm.Name,
                Detail = vm.Detail
            };

            await _categoryService.UpdateAsync(category);
            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteAsync(id);
            return RedirectToAction(nameof(List));
        }
    }
}
