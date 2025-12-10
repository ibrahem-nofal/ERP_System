using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ERP_System.Models;
using ERP_System.ViewModels;
using ERP_System.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace ERP_System.Controllers
{
    [Authorize]
    public class DefineSupplierController : Controller
    {
        private readonly ISupplierService _supplierService;

        public DefineSupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        public async Task<IActionResult> List()
        {
            var suppliers = await _supplierService.GetAllAsync();
            return View(suppliers);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(AddSuppVm advm)
        {
            if (!ModelState.IsValid) return View(advm);

            var supp = new Supplier
            {
                Name = advm.Name,
                ManagerName = advm.ManagerName,
                ManagerPhone = advm.ManagerPhone,
                OwnerName = advm.OwnerName,
                OwnerPhone = advm.OwnerPhone,
                Address = advm.Address
            };

            await _supplierService.AddAsync(supp, advm.Phones);

            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _supplierService.GetByIdAsync(id);
            if (supplier == null) return NotFound();

            var vm = new AddSuppVm
            {
                Name = supplier.Name,
                ManagerName = supplier.ManagerName,
                ManagerPhone = supplier.ManagerPhone,
                OwnerName = supplier.OwnerName,
                OwnerPhone = supplier.OwnerPhone,
                Address = supplier.Address,
                Phones = supplier.Phones?.Select(p => p.Phone).ToList() ?? new List<string>()
            };

            ViewBag.Id = id;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AddSuppVm vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Id = id;
                return View(vm);
            }

            var supplier = new Supplier
            {
                Id = id,
                Name = vm.Name,
                ManagerName = vm.ManagerName,
                ManagerPhone = vm.ManagerPhone,
                OwnerName = vm.OwnerName,
                OwnerPhone = vm.OwnerPhone,
                Address = vm.Address
            };

            await _supplierService.UpdateAsync(supplier, vm.Phones);
            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Details(int id)
        {
            var supplier = await _supplierService.GetByIdAsync(id);
            if (supplier == null) return NotFound();
            return View(supplier);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _supplierService.DeleteAsync(id);
            return RedirectToAction(nameof(List));
        }
    }
}
