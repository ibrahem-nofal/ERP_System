using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP_System.Models;
using ERP_System.Data;
using ERP_System.ViewModels;

namespace ERP_System.Controllers
{
    [Authorize]
    public class DefineCustomerController : Controller
    {
        private readonly AppDbContext _context;

        public DefineCustomerController(AppDbContext context)
        {
            _context = context;
        }

        // List all customers
        public async Task<IActionResult> List()
        {
            var customers = await _context.Customers
                .Include(c => c.Phones)
                .ToListAsync();
            return View(customers);
        }

        // Create new customer (GET)
        public IActionResult Index()
        {
            return View();
        }

        // Create new customer (POST)
        [HttpPost]
        public IActionResult Index(AddCustVm advm)
        {
            var cust = new Customer
            {
                Name = advm.Name,
                Address = advm.Address,
                BirthDate = advm.BirthDate,
                Gender = ((Gender)advm.Gender).ToString(),
                OtherDetails = advm.OtherDetails,
                StartDate = advm.StartDate
            };

            _context.Customers.Add(cust);

            if (advm.Phones != null)
            {
                foreach (var ph in advm.Phones)
                {
                    var custPhone = new CustomerPhone
                    {
                        Customer = cust,
                        Phone = ph
                    };
                    _context.CustomerPhones.Add(custPhone);
                }
            }

            _context.SaveChanges();
            return RedirectToAction("List");
        }

        // View customer details
        public async Task<IActionResult> Details(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Phones)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // Edit customer (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Phones)
                .FirstOrDefaultAsync(c => c.Id == id);

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
            var customer = await _context.Customers
                .Include(c => c.Phones)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            customer.Name = advm.Name;
            customer.Gender = ((Gender)advm.Gender).ToString();
            customer.Address = advm.Address;
            customer.StartDate = advm.StartDate;
            customer.BirthDate = advm.BirthDate;
            customer.OtherDetails = advm.OtherDetails;

            // Update phones
            _context.CustomerPhones.RemoveRange(customer.Phones);
            if (advm.Phones != null)
            {
                foreach (var ph in advm.Phones)
                {
                    var custPhone = new CustomerPhone
                    {
                        CustomerId = customer.Id,
                        Phone = ph
                    };
                    _context.CustomerPhones.Add(custPhone);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("List");
        }

        // Delete customer
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Phones)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return RedirectToAction("List");
        }
    }
}
