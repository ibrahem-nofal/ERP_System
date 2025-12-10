using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ERP_System.Models;
using ERP_System.ViewModels;
using ERP_System.Services.Interfaces;

namespace ERP_System.Controllers
{
    [Authorize]
    public class DefineCustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public DefineCustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // List all customers
        public async Task<IActionResult> List()
        {
            var customers = await _customerService.GetAllAsync();
            return View(customers);
        }

        // Create new customer (GET)
        public IActionResult Index()
        {
            return View();
        }

        // Create new customer (POST)
        [HttpPost]
        public async Task<IActionResult> Index(AddCustVm advm)
        {
            if (!ModelState.IsValid) return View(advm);

            var cust = new Customer
            {
                Name = advm.Name,
                Address = advm.Address,
                BirthDate = advm.BirthDate,
                Gender = ((Gender)advm.Gender).ToString(),
                OtherDetails = advm.OtherDetails,
                StartDate = advm.StartDate
            };

            try
            {
                await _customerService.AddAsync(cust, advm.Phones);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "حدث خطأ أثناء الحفظ. يرجى التأكد من صحة البيانات.");
                return View(advm);
            }
            return RedirectToAction("List");
        }

        // View customer details
        public async Task<IActionResult> Details(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // Edit customer (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            var viewModel = new AddCustVm
            {
                Name = customer.Name,
                Gender = Enum.Parse<Gender>(customer.Gender) == Gender.Male ? 0 : 1,
                Address = customer.Address,
                StartDate = customer.StartDate,
                BirthDate = customer.BirthDate,
                OtherDetails = customer.OtherDetails,
                Phones = customer.Phones?.Select(p => p.Phone).ToList() ?? new List<string>()
            };

            ViewBag.CustomerId = id;
            return View(viewModel);
        }

        // Edit customer (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(int id, AddCustVm advm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CustomerId = id;
                return View(advm);
            }

            var customer = new Customer
            {
                Id = id,
                Name = advm.Name,
                Gender = ((Gender)advm.Gender).ToString(),
                Address = advm.Address,
                StartDate = advm.StartDate,
                BirthDate = advm.BirthDate,
                OtherDetails = advm.OtherDetails
            };

            await _customerService.UpdateAsync(customer, advm.Phones);

            return RedirectToAction("List");
        }

        // Delete customer
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _customerService.DeleteAsync(id);
            return RedirectToAction("List");
        }
    }
}
