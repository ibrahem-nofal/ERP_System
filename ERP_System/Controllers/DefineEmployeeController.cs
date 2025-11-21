using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP_System.Models;
using ERP_System.Data;
using ERP_System.ViewModels;
using System.Collections;

namespace ERP_System.Controllers
{
    public class DefineEmployeeController : Controller
    {
        private readonly AppDbContext _context;

        public DefineEmployeeController(AppDbContext context)
        {
            _context = context;
        }

        // List all employees
        public async Task<IActionResult> List()
        {
            var employees = await _context.Employees
                .Include(e => e.Phones)
                .Include(e => e.Image)
                .ToListAsync();
            return View(employees);
        }

        // Create new employee (GET)
        public IActionResult Index()
        {
            return View();
        }

        // Create new employee (POST)
        [HttpPost]
        public IActionResult Index(AddEmpVm advm)
        {
            byte[] imageBytes = null;

            if (advm.EmpImage != null)
            {
                using (var ms = new MemoryStream())
                {
                    advm.EmpImage.CopyTo(ms);
                    imageBytes = ms.ToArray();
                }
            }


            var emp = new Employee
            {
                Name = advm.Name,
                RoleType = ((RoleType)advm.RoleType).ToString(),
                Gender = ((Gender)advm.Gender).ToString(),
                Address = advm.Address,
                IdNumber = advm.IdNumber,
                BirthDate = advm.BirthDate,
                Qualification = ((Qualification)advm.Qualification).ToString(),
                State = ((State)advm.State).ToString()
            };

            _context.Employees.Add(emp);

            foreach (var ph in advm.Phones)
            {
                var empPhone = new EmpPhone
                {
                    Employee = emp,
                    Phone = ph
                };
                _context.EmpPhones.Add(empPhone);
            }

            if (imageBytes != null)
            {
                var img = new EmpImage
                {
                    Employee = emp,
                    EmpImageData = imageBytes
                };

                _context.EmpImages.Add(img);
            }


            _context.SaveChanges();

            return RedirectToAction("List");
        }

        // View employee details
        public async Task<IActionResult> Details(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Phones)
                .Include(e => e.Image)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // Edit employee (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Phones)
                .Include(e => e.Image)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            var viewModel = new AddEmpVm
            {
                Name = employee.Name,
                RoleType = Enum.Parse<RoleType>(employee.RoleType) == RoleType.Delegate ? 0 : 1,
                Gender = Enum.Parse<Gender>(employee.Gender) == Gender.Male ? 0 : 1,
                Address = employee.Address,
                IdNumber = employee.IdNumber,
                BirthDate = employee.BirthDate,
                Qualification = (int)Enum.Parse<Qualification>(employee.Qualification),
                State = (int)Enum.Parse<State>(employee.State),
                Phones = employee.Phones?.Select(p => p.Phone).ToList() ?? new List<string>()
            };

            ViewBag.EmployeeId = id;
            return View(viewModel);
        }

        // Edit employee (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(int id, AddEmpVm advm)
        {
            var employee = await _context.Employees
                .Include(e => e.Phones)
                .Include(e => e.Image)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            // Update employee properties
            employee.Name = advm.Name;
            employee.RoleType = ((RoleType)advm.RoleType).ToString();
            employee.Gender = ((Gender)advm.Gender).ToString();
            employee.Address = advm.Address;
            employee.IdNumber = advm.IdNumber;
            employee.BirthDate = advm.BirthDate;
            employee.Qualification = ((Qualification)advm.Qualification).ToString();
            employee.State = ((State)advm.State).ToString();

            // Update phones
            _context.EmpPhones.RemoveRange(employee.Phones);
            foreach (var ph in advm.Phones)
            {
                var empPhone = new EmpPhone
                {
                    EmpId = employee.Id,
                    Phone = ph
                };
                _context.EmpPhones.Add(empPhone);
            }

            // Update image if provided
            if (advm.EmpImage != null)
            {
                byte[] imageBytes;
                using (var ms = new MemoryStream())
                {
                    advm.EmpImage.CopyTo(ms);
                    imageBytes = ms.ToArray();
                }

                if (employee.Image != null)
                {
                    employee.Image.EmpImageData = imageBytes;
                }
                else
                {
                    var img = new EmpImage
                    {
                        EmpId = employee.Id,
                        EmpImageData = imageBytes
                    };
                    _context.EmpImages.Add(img);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("List");
        }

        // Delete employee
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Phones)
                .Include(e => e.Image)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction("List");
        }
    }
}
