using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ERP_System.Models;
using ERP_System.Data;
using ERP_System.ViewModels;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> List()
        {
            var stores = await _context.Stores.ToListAsync();
            return View(stores);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(AddStoreVm advm)
        {
            if (!ModelState.IsValid) return View(advm);

            var store = new Store
            {
                Name = advm.Name,
                Address = advm.Address,
                OtherDetails = advm.OtherDetails,
                Phone = advm.Phone
            };

            _context.Stores.Add(store);
            _context.SaveChanges();
            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var store = await _context.Stores.FindAsync(id);
            if (store == null) return NotFound();

            var vm = new AddStoreVm
            {
                Name = store.Name,
                Address = store.Address,
                OtherDetails = store.OtherDetails,
                Phone = store.Phone
            };
            ViewBag.Id = id;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AddStoreVm vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Id = id;
                return View(vm);
            }

            var store = await _context.Stores.FindAsync(id);
            if (store == null) return NotFound();

            store.Name = vm.Name;
            store.Address = vm.Address;
            store.OtherDetails = vm.OtherDetails;
            store.Phone = vm.Phone;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Details(int id)
        {
            var store = await _context.Stores.FindAsync(id);
            if (store == null) return NotFound();
            return View(store);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var store = await _context.Stores.FindAsync(id);
            if (store != null)
            {
                _context.Stores.Remove(store);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(List));
        }
    }
}
