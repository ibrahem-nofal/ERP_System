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
    public class DefineStoreController : Controller
    {
        private readonly IStoreService _storeService;

        public DefineStoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        // ===================== List =====================
        public async Task<IActionResult> List()
        {
            var stores = await _storeService.GetAllAsync();
            return View(stores);
        }

        // ===================== Create (GET) =====================
        public IActionResult Index()
        {
            return View(new AddStoreVm());
        }

        // ===================== Create (POST) =====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(AddStoreVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            // منع تكرار اسم المخزن
            if (await _storeService.IsNameExistsAsync(vm.Name))
            {
                ModelState.AddModelError(nameof(vm.Name), "اسم المخزن مستخدم بالفعل. الرجاء اختيار اسم آخر.");
                return View(vm);
            }

            var store = new Store
            {
                Name = vm.Name,
                Address = vm.Address,
                ManagerName = vm.ManagerName,
                Email = vm.Email,
                IsActive = vm.IsActive,
                OpeningHours = vm.OpeningHours,
                Notes = vm.Notes
            };

            byte[]? imageBytes = null;
            if (vm.Image != null)
            {
                using (var ms = new MemoryStream())
                {
                    await vm.Image.CopyToAsync(ms);
                    imageBytes = ms.ToArray();
                }
            }

            await _storeService.AddAsync(store, vm.Phones, imageBytes);

            return RedirectToAction(nameof(List));
        }

        // ===================== Details =====================
        public async Task<IActionResult> Details(int id)
        {
            var store = await _storeService.GetByIdAsync(id);
            if (store == null) return NotFound();
            return View(store);
        }

        // ===================== Edit (GET) =====================
        public async Task<IActionResult> Edit(int id)
        {
            var store = await _storeService.GetByIdAsync(id);
            if (store == null) return NotFound();

            var vm = new AddStoreVm
            {
                Name = store.Name,
                Address = store.Address,
                ManagerName = store.ManagerName,
                Phones = store.Phones?.Select(p => p.Phone).ToList() ?? new System.Collections.Generic.List<string>(),
                Email = store.Email,
                IsActive = store.IsActive,
                OpeningHours = store.OpeningHours,
                Notes = store.Notes
            };

            // إذا توجد صورة حالية ضعها في ViewBag لعرضها في الفيو
            var firstImage = store.Images?.FirstOrDefault();
            if (firstImage != null && firstImage.ImageData != null && firstImage.ImageData.Length > 0)
            {
                ViewBag.ExistingImageBase64 = $"data:image/png;base64,{Convert.ToBase64String(firstImage.ImageData)}";
            }

            ViewBag.Id = id;
            return View(vm);
        }

        // ===================== Edit (POST) =====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AddStoreVm vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Id = id;
                return View(vm);
            }

            var store = await _storeService.GetByIdAsync(id);
            if (store == null) return NotFound();

            // منع تكرار اسم المخزن لغير هذا السجل
            if (await _storeService.IsNameExistsAsync(vm.Name, id))
            {
                ModelState.AddModelError(nameof(vm.Name), "اسم المخزن مستخدم من قبل مخزن آخر.");
                ViewBag.Id = id;
                return View(vm);
            }

            var updateStore = new Store
            {
                Id = id,
                Name = vm.Name,
                Address = vm.Address,
                ManagerName = vm.ManagerName,
                Email = vm.Email,
                IsActive = vm.IsActive,
                OpeningHours = vm.OpeningHours,
                Notes = vm.Notes
            };

            byte[]? imageBytes = null;
            if (vm.Image != null)
            {
                using (var ms = new MemoryStream())
                {
                    await vm.Image.CopyToAsync(ms);
                    imageBytes = ms.ToArray();
                }
            }

            // Note: Service handles all logic (phones, images, properties)
            await _storeService.UpdateAsync(updateStore, vm.Phones, imageBytes);

            return RedirectToAction(nameof(List));
        }

        // ===================== Delete (POST) =====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _storeService.DeleteAsync(id);
            return RedirectToAction(nameof(List));
        }
    }
}
