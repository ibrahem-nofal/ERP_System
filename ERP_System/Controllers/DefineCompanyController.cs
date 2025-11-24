using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ERP_System.Models;
using ERP_System.Data;
using ERP_System.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ERP_System.Controllers
{
    [Authorize]
    public class DefineCompanyController : Controller
    {
        private readonly AppDbContext _context;

        public DefineCompanyController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> List()
        {
            var companies = await _context.Companies.ToListAsync();
            return View(companies);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(AddCompVm advm)
        {
            if (!ModelState.IsValid) return View(advm);

            var comp = new Company
            {
                Code = advm.Code,
                Name = advm.Name,
                Address = advm.Address,
                DateCreated = advm.DateCreated,
                OtherDetails = advm.OtherDetails
            };

            _context.Companies.Add(comp);
            _context.SaveChanges(); // Save to get ID

            if (advm.Phones != null)
            {
                foreach (var ph in advm.Phones)
                {
                    if (!string.IsNullOrWhiteSpace(ph))
                    {
                        var compPhone = new CompanyPhone
                        {
                            Company = comp,
                            Phone = ph
                        };
                        _context.CompanyPhones.Add(compPhone);
                    }
                }
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var company = await _context.Companies.Include(c => c.Phones).FirstOrDefaultAsync(c => c.Id == id);
            if (company == null) return NotFound();

            var vm = new AddCompVm
            {
                Code = company.Code,
                Name = company.Name,
                Address = company.Address,
                DateCreated = company.DateCreated,
                OtherDetails = company.OtherDetails,
                Phones = company.Phones.Select(p => p.Phone).ToList()
            };

            ViewBag.Id = id;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AddCompVm vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Id = id;
                return View(vm);
            }

            var company = await _context.Companies.Include(c => c.Phones).FirstOrDefaultAsync(c => c.Id == id);
            if (company == null) return NotFound();

            company.Code = vm.Code;
            company.Name = vm.Name;
            company.Address = vm.Address;
            company.DateCreated = vm.DateCreated;
            company.OtherDetails = vm.OtherDetails;

            // Update phones
            _context.CompanyPhones.RemoveRange(company.Phones);

            if (vm.Phones != null)
            {
                foreach (var ph in vm.Phones)
                {
                    if (!string.IsNullOrWhiteSpace(ph))
                    {
                        _context.CompanyPhones.Add(new CompanyPhone { Company = company, Phone = ph });
                    }
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Details(int id)
        {
            var company = await _context.Companies.Include(c => c.Phones).FirstOrDefaultAsync(c => c.Id == id);
            if (company == null) return NotFound();
            return View(company);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company != null)
            {
                _context.Companies.Remove(company);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(List));
        }
    }
}
