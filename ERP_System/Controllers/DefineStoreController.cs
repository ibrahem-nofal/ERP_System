using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP_System.Data;
using ERP_System.Models;
using ERP_System.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ERP_System.Controllers
{
    [Authorize]
    public class DefineStoreController : Controller
    {
        private readonly AppDbContext _context;

        public DefineStoreController(AppDbContext context)
        {
            _context = context;
        }

        // ===================== List =====================
        public async Task<IActionResult> List()
        {
            var stores = await _context.Stores
                .AsNoTracking()
                .Include(s => s.Phones)
                .Include(s => s.Images)
                .ToListAsync();

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
            if (await _context.Stores.AnyAsync(s => s.Name == vm.Name))
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

            // قم بإضافة المخزن أولاً حتى يكون له Id (لتعيين FK للهواتف والصور)
            _context.Stores.Add(store);

            // أضف الهواتف ككيانات منفصلة (StorePhone)
            if (vm.Phones != null && vm.Phones.Any())
            {
                foreach (var ph in vm.Phones.Where(p => !string.IsNullOrWhiteSpace(p)))
                {
                    _context.Set<StorePhone>().Add(new StorePhone
                    {
                        Store = store,
                        Phone = ph.Trim()
                    });
                }
            }

            // أضف الصورة إن وُجدت
            if (vm.Image != null)
            {
                using (var ms = new MemoryStream())
                {
                    await vm.Image.CopyToAsync(ms);
                    var bytes = ms.ToArray();

                    _context.Set<StoreImage>().Add(new StoreImage
                    {
                        Store = store,
                        ImageData = bytes
                    });
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "حدث خطأ أثناء حفظ المخزن. حاول مرة أخرى أو تواصل مع الدعم.");
                return View(vm);
            }

            return RedirectToAction(nameof(List));
        }

        // ===================== Details =====================
        public async Task<IActionResult> Details(int id)
        {
            var store = await _context.Stores
                .AsNoTracking()
                .Include(s => s.Phones)
                .Include(s => s.Images)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (store == null) return NotFound();

            return View(store);
        }

        // ===================== Edit (GET) =====================
        public async Task<IActionResult> Edit(int id)
        {
            var store = await _context.Stores
                .Include(s => s.Phones)
                .Include(s => s.Images)
                .FirstOrDefaultAsync(s => s.Id == id);

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

            var store = await _context.Stores
                .Include(s => s.Phones)
                .Include(s => s.Images)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (store == null) return NotFound();

            // منع تكرار اسم المخزن لغير هذا السجل
            if (await _context.Stores.AnyAsync(s => s.Name == vm.Name && s.Id != id))
            {
                ModelState.AddModelError(nameof(vm.Name), "اسم المخزن مستخدم من قبل مخزن آخر.");
                ViewBag.Id = id;
                return View(vm);
            }

            // حدّث الحقول الرئيسية
            store.Name = vm.Name;
            store.Address = vm.Address;
            store.ManagerName = vm.ManagerName;
            store.Email = vm.Email;
            store.IsActive = vm.IsActive;
            store.OpeningHours = vm.OpeningHours;
            store.Notes = vm.Notes;

            // ===== تحديث الهواتف =====
            // نحذف الهواتف القديمة ثم نضيف الجديدة لضمان التزامن التام
            if (store.Phones != null && store.Phones.Any())
            {
                _context.StorePhones.RemoveRange(store.Phones);
            }

            if (vm.Phones != null && vm.Phones.Any())
            {
                foreach (var ph in vm.Phones.Where(p => !string.IsNullOrWhiteSpace(p)))
                {
                    _context.StorePhones.Add(new StorePhone
                    {
                        StoreId = store.Id,
                        Phone = ph.Trim()
                    });
                }
            }

            // ===== تحديث الصورة (إن أُرسلت صورة جديدة) =====
            if (vm.Image != null)
            {
                using (var ms = new MemoryStream())
                {
                    await vm.Image.CopyToAsync(ms);
                    var bytes = ms.ToArray();

                    // استخرج أول صورة موجودة إن وُجدت ثم حدّثها، وإلا أضف سجل جديد
                    var existingImage = store.Images?.FirstOrDefault();
                    if (existingImage != null)
                    {
                        existingImage.ImageData = bytes;
                        _context.StoreImages.Update(existingImage);
                    }
                    else
                    {
                        _context.StoreImages.Add(new StoreImage
                        {
                            StoreId = store.Id,
                            ImageData = bytes
                        });
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "تعذّر حفظ التعديلات. قد توجد قيود في قاعدة البيانات.");
                ViewBag.Id = id;
                return View(vm);
            }

            return RedirectToAction(nameof(List));
        }

        // ===================== Delete (POST) =====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var store = await _context.Stores
                .Include(s => s.Phones)
                .Include(s => s.Images)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (store == null) return RedirectToAction(nameof(List));

            try
            {
                // احذف الجداول المرتبطة أولاً إن كانت هناك FK بدون Cascade
                if (store.Phones != null && store.Phones.Any())
                    _context.StorePhones.RemoveRange(store.Phones);

                if (store.Images != null && store.Images.Any())
                    _context.StoreImages.RemoveRange(store.Images);

                _context.Stores.Remove(store);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] = "تعذّر حذف المخزن — قد توجد بيانات مرتبطة به.";
                return RedirectToAction(nameof(List));
            }

            return RedirectToAction(nameof(List));
        }
    }
}
