using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ERP_System.Models;
using ERP_System.Data;
using ERP_System.ViewModels;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace ERP_System.Controllers
{
    [Authorize]
    public class DefineSupplierController : Controller
    {
        private readonly AppDbContext _context;

        public DefineSupplierController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> List()
        {
            var suppliers = await _context.Suppliers.ToListAsync();
            return View(suppliers);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(AddSuppVm advm)
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
            _context.Suppliers.Add(supp);
            _context.SaveChanges(); // Save to get ID

            if (advm.Phones != null)
            {
                foreach (var ph in advm.Phones)
                {
                    if (!string.IsNullOrWhiteSpace(ph))
                    {
                        var suppPhone = new SupplierPhone
                        {
                            Supplier = supp,
                            Phone = ph
                        };
                        _context.SupplierPhones.Add(suppPhone);
                    }
                }
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _context.Suppliers.Include(s => s.Phones).FirstOrDefaultAsync(s => s.Id == id);
            if (supplier == null) return NotFound();

            var vm = new AddSuppVm
            {
                Name = supplier.Name,
                ManagerName = supplier.ManagerName,
                ManagerPhone = supplier.ManagerPhone,
                OwnerName = supplier.OwnerName,
                OwnerPhone = supplier.OwnerPhone,
                Address = supplier.Address,
                Phones = supplier.Phones.Select(p => p.Phone).ToList()
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

            var supplier = await _context.Suppliers.Include(s => s.Phones).FirstOrDefaultAsync(s => s.Id == id);
            if (supplier == null) return NotFound();

            supplier.Name = vm.Name;
            supplier.ManagerName = vm.ManagerName;
            supplier.ManagerPhone = vm.ManagerPhone;
            supplier.OwnerName = vm.OwnerName;
            supplier.OwnerPhone = vm.OwnerPhone;
            supplier.Address = vm.Address;

            // Update phones - simple strategy: remove all and re-add
            _context.SupplierPhones.RemoveRange(supplier.Phones);

            if (vm.Phones != null)
            {
                foreach (var ph in vm.Phones)
                {
                    if (!string.IsNullOrWhiteSpace(ph))
                    {
                        _context.SupplierPhones.Add(new SupplierPhone { Supplier = supplier, Phone = ph });
                    }
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Details(int id)
        {
            var supplier = await _context.Suppliers.Include(s => s.Phones).FirstOrDefaultAsync(s => s.Id == id);
            if (supplier == null) return NotFound();
            return View(supplier);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(List));
        }
    }
}
