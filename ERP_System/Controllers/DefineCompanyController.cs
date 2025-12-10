using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ERP_System.Models;
using ERP_System.ViewModels;
using ERP_System.Services.Interfaces;

namespace ERP_System.Controllers
{
    [Authorize]
    public class DefineCompanyController : Controller
    {
        private readonly ICompanyService _companyService;

        public DefineCompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }
        public async Task<IActionResult> List()
        {
            var companies = await _companyService.GetAllAsync();
            return View(companies);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(AddCompVm advm)
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

            await _companyService.AddAsync(comp, advm.Phones);

            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var company = await _companyService.GetByIdAsync(id);
            if (company == null) return NotFound();

            var vm = new AddCompVm
            {
                Code = company.Code,
                Name = company.Name,
                Address = company.Address,
                DateCreated = company.DateCreated,
                OtherDetails = company.OtherDetails,
                Phones = company.Phones?.Select(p => p.Phone).ToList() ?? new List<string>()
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

            var company = new Company
            {
                Id = id,
                Code = vm.Code,
                Name = vm.Name,
                Address = vm.Address,
                DateCreated = vm.DateCreated,
                OtherDetails = vm.OtherDetails
            };

            // Note: Service handles checking if exists and updating phone logic
            await _companyService.UpdateAsync(company, vm.Phones);

            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Details(int id)
        {
            var company = await _companyService.GetByIdAsync(id);
            if (company == null) return NotFound();
            return View(company);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _companyService.DeleteAsync(id);
            return RedirectToAction(nameof(List));
        }
    }
}
