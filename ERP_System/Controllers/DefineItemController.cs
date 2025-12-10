using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ERP_System.Models;
using ERP_System.ViewModels;
using ERP_System.Services.Interfaces;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ERP_System.Controllers
{
    [Authorize]
    public class DefineItemController : Controller
    {
        private readonly IItemService _itemService;
        private readonly ICompanyService _companyService;
        private readonly IStoreService _storeService;
        private readonly IUnitService _unitService;
        private readonly ICategoryService _categoryService;

        public DefineItemController(
            IItemService itemService,
            ICompanyService companyService,
            IStoreService storeService,
            IUnitService unitService,
            ICategoryService categoryService)
        {
            _itemService = itemService;
            _companyService = companyService;
            _storeService = storeService;
            _unitService = unitService;
            _categoryService = categoryService;
        }

        // List all items
        public async Task<IActionResult> List()
        {
            var items = await _itemService.GetAllAsync();
            return View(items);
        }

        // Create new item (GET)
        public async Task<IActionResult> Index()
        {
            await LoadViewBagDataAsync();
            return View(new AddItemVm());
        }

        // Create new item (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(AddItemVm advm)
        {
            // التحقق من صحة النموذج
            if (!ModelState.IsValid)
            {
                await LoadViewBagDataAsync();
                return View(advm);
            }

            byte[]? imageBytes = null;
            if (advm.Image != null)
            {
                using var ms = new MemoryStream();
                await advm.Image.CopyToAsync(ms);
                imageBytes = ms.ToArray();
            }

            var item = new Item
            {
                Name = advm.Name?.Trim(),
                Description = advm.Description?.Trim(),
                CompanyMade = advm.CompanyMade,
                DefaultStore = advm.DefaultStore,
                BuyPrice = advm.BuyPrice,
                IsActiveBuy = advm.IsActiveBuy,
                IsActiveSale = advm.IsActiveSale,
                MinimumQuantity = advm.MinimumQuantity,
                MinQuantitySale = advm.MinQuantitySale,
                PreventDiscount = advm.PreventDiscount,
                PreventFraction = advm.PreventFraction,
                SalePrice = advm.SalePrice,
                UnitNumber = advm.UnitNumber
            };

            await _itemService.AddAsync(item, advm.Codes, advm.CategoryIds, imageBytes);

            return RedirectToAction("List");
        }

        // View item details
        public async Task<IActionResult> Details(int id)
        {
            var item = await _itemService.GetByIdAsync(id);

            if (item == null) return NotFound();

            return View(item);
        }

        // Edit item (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _itemService.GetByIdAsync(id);
            if (item == null) return NotFound();

            var categoryIds = await _itemService.GetCategoryIdsAsync(id);

            var viewModel = new AddItemVm
            {
                Name = item.Name,
                Description = item.Description,
                IsActiveBuy = item.IsActiveBuy ?? false,
                IsActiveSale = item.IsActiveSale ?? false,
                CompanyMade = item.CompanyMade,
                DefaultStore = item.DefaultStore,
                UnitNumber = item.UnitNumber,
                MinimumQuantity = item.MinimumQuantity,
                MinQuantitySale = item.MinQuantitySale,
                PreventFraction = item.PreventFraction,
                PreventDiscount = item.PreventDiscount,
                BuyPrice = item.BuyPrice,
                SalePrice = item.SalePrice,
                Codes = item.Codes?.Select(c => c.ItemCodeValue).ToList() ?? new List<string>(),
                CategoryIds = categoryIds
            };

            // تحميل الصورة لعرضها
            if (item.Images != null && item.Images.Any() && item.Images.First().ItemImageData != null)
            {
                ViewBag.ImageBase64 = Convert.ToBase64String(item.Images.First().ItemImageData);
            }

            await LoadViewBagDataAsync();
            ViewBag.ItemId = id;
            return View(viewModel);
        }

        // Edit item (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AddItemVm advm)
        {
            if (!ModelState.IsValid)
            {
                await LoadViewBagDataAsync();
                ViewBag.ItemId = id;
                return View(advm);
            }

            var item = new Item
            {
                Id = id,
                Name = advm.Name?.Trim(),
                Description = advm.Description?.Trim(),
                IsActiveBuy = advm.IsActiveBuy,
                IsActiveSale = advm.IsActiveSale,
                CompanyMade = advm.CompanyMade,
                DefaultStore = advm.DefaultStore,
                UnitNumber = advm.UnitNumber,
                MinimumQuantity = advm.MinimumQuantity,
                MinQuantitySale = advm.MinQuantitySale,
                PreventFraction = advm.PreventFraction,
                PreventDiscount = advm.PreventDiscount,
                BuyPrice = advm.BuyPrice,
                SalePrice = advm.SalePrice
            };

            byte[]? imageBytes = null;
            if (advm.Image != null)
            {
                using (var ms = new MemoryStream())
                {
                    await advm.Image.CopyToAsync(ms);
                    imageBytes = ms.ToArray();
                }
            }

            await _itemService.UpdateAsync(item, advm.Codes, advm.CategoryIds, imageBytes);

            return RedirectToAction("List");
        }

        // Delete item
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _itemService.DeleteAsync(id);
            return RedirectToAction("List");
        }

        // Helper method to load ViewBag data
        private async Task LoadViewBagDataAsync()
        {
            // Company VM
            var companyList = await _companyService.GetAllAsync();
            var companies = companyList.Select(s => new CompnayVm
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();

            // Store VM
            var storeList = await _storeService.GetAllAsync();
            var stores = storeList.Select(s => new StoreVm
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();

            // Unit VM
            var unitList = await _unitService.GetAllAsync();
            var units = unitList.Select(s => new UnitVm
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();

            // Category VM
            var categoryList = await _categoryService.GetAllAsync();
            var cats = categoryList.Select(s => new CategoryVm
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();

            ViewBag.companies = companies;
            ViewBag.stores = stores;
            ViewBag.units = units;
            ViewBag.categories = cats;
        }
    }
}
