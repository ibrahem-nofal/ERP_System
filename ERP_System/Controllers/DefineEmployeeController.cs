using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP_System.Models;
using ERP_System.Data;
using ERP_System.ViewModels;
using System.Collections;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ERP_System.Controllers
{
    [Authorize]
    public class DefineEmployeeController : Controller
    {
        private readonly AppDbContext _context;

        public DefineEmployeeController(AppDbContext context)
        {
            _context = context;
        }

        // ??? ???? ????????
        public async Task<IActionResult> List()
        {
            var employees = await _context.Employees
                .Include(e => e.Phones)
                .Include(e => e.Image)
                .ToListAsync();
            return View(employees);
        }

        // ???? ????? ???? ???? (GET)
        public IActionResult Index()
        {
            return View(new AddEmpVm());
        }

        // ????? ???? ???? (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(AddEmpVm advm)
        {
            // ????? ????????
            advm.IdNumber = advm.IdNumber?.Trim();
            advm.Name = advm.Name?.Trim();
            advm.Address = advm.Address?.Trim();

            // ???? ?? ?????
            if (string.IsNullOrWhiteSpace(advm.Name))
                ModelState.AddModelError(nameof(advm.Name), "?????? ????? ??? ??????.");

            // ???? ?? ??? ??????
            if (string.IsNullOrWhiteSpace(advm.IdNumber))
                ModelState.AddModelError(nameof(advm.IdNumber), "?????? ????? ??? ??????.");

            // ???? ?? ????? ??? ??????
            if (!string.IsNullOrWhiteSpace(advm.IdNumber) &&
                await _context.Employees.AnyAsync(e => e.IdNumber == advm.IdNumber))
            {
                ModelState.AddModelError(nameof(advm.IdNumber), "??? ?????? ??? ?????? ?? ???? ?????? ????? ??? ???.");
            }

            if (!ModelState.IsValid)
            {
                return View(advm);
            }

            byte[] imageBytes = null;
            if (advm.EmpImage != null)
            {
                using (var ms = new MemoryStream())
                {
                    await advm.EmpImage.CopyToAsync(ms);
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

            try
            {
                _context.Employees.Add(emp);

                if (advm.Phones != null)
                {
                    foreach (var ph in advm.Phones.Where(p => !string.IsNullOrWhiteSpace(p)))
                    {
                        var empPhone = new EmpPhone
                        {
                            Employee = emp,
                            Phone = ph.Trim()
                        };
                        _context.EmpPhones.Add(empPhone);
                    }
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

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "????? ??? ????????. ?? ???? ???? ??? ???? ???? ?? ????? ?? ????? ????????.");
                return View(advm);
            }

            return RedirectToAction("List");
        }

        // ??? ?????? ????
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

        // ??? ???? ????? ???? (GET)
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
                RoleType = Enum.TryParse<RoleType>(employee.RoleType, out var r) ? (int)r : 0,
                Gender = Enum.TryParse<Gender>(employee.Gender, out var g) ? (int)g : 0,
                Address = employee.Address,
                IdNumber = employee.IdNumber,
                BirthDate = employee.BirthDate,
                Qualification = Enum.TryParse<Qualification>(employee.Qualification, out var q) ? (int)q : 0,
                State = Enum.TryParse<State>(employee.State, out var s) ? (int)s : 0,
                Phones = employee.Phones?.Select(p => p.Phone).ToList() ?? new List<string>()
            };

            ViewBag.EmployeeId = id;
            return View(viewModel);
        }

        // ????? ???? (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AddEmpVm advm)
        {
            advm.IdNumber = advm.IdNumber?.Trim();
            advm.Name = advm.Name?.Trim();
            advm.Address = advm.Address?.Trim();

            var employee = await _context.Employees
                .Include(e => e.Phones)
                .Include(e => e.Image)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            // ?????? ?? ??????
            if (string.IsNullOrWhiteSpace(advm.Name))
                ModelState.AddModelError(nameof(advm.Name), "?????? ????? ??? ??????.");

            if (string.IsNullOrWhiteSpace(advm.IdNumber))
                ModelState.AddModelError(nameof(advm.IdNumber), "?????? ????? ??? ??????.");

            // ??? ????? ??? ?????? ?? ?????? ??? ??? ??? ??????
            if (!string.IsNullOrWhiteSpace(advm.IdNumber) &&
                await _context.Employees.AnyAsync(e => e.IdNumber == advm.IdNumber && e.Id != id))
            {
                ModelState.AddModelError(nameof(advm.IdNumber), "??? ?????? ??? ?????? ?? ??? ???? ???.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.EmployeeId = id;
                return View(advm);
            }

            // ????? ?????? ??????
            employee.Name = advm.Name;
            employee.RoleType = ((RoleType)advm.RoleType).ToString();
            employee.Gender = ((Gender)advm.Gender).ToString();
            employee.Address = advm.Address;
            employee.IdNumber = advm.IdNumber;
            employee.BirthDate = advm.BirthDate;
            employee.Qualification = ((Qualification)advm.Qualification).ToString();
            employee.State = ((State)advm.State).ToString();

            // ????? ???????
            _context.EmpPhones.RemoveRange(employee.Phones ?? Enumerable.Empty<EmpPhone>());

            if (advm.Phones != null)
            {
                foreach (var ph in advm.Phones.Where(p => !string.IsNullOrWhiteSpace(p)))
                {
                    _context.EmpPhones.Add(new EmpPhone
                    {
                        EmpId = employee.Id,
                        Phone = ph.Trim()
                    });
                }
            }

            // ????? ??????
            if (advm.EmpImage != null)
            {
                using (var ms = new MemoryStream())
                {
                    await advm.EmpImage.CopyToAsync(ms);
                    var imageBytes = ms.ToArray();

                    if (employee.Image != null)
                    {
                        employee.Image.EmpImageData = imageBytes;
                    }
                    else
                    {
                        _context.EmpImages.Add(new EmpImage
                        {
                            EmpId = employee.Id,
                            EmpImageData = imageBytes
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
                ModelState.AddModelError("", "????? ??? ?????????. ?? ???? ???? ??? ???? ???? ?? ????? ?? ????? ????????.");
                ViewBag.EmployeeId = id;
                return View(advm);
            }

            return RedirectToAction("List");
        }

        // ??? ????
        [HttpPost]
        [ValidateAntiForgeryToken]
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

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] = "?? ???? ??? ?????? ???? ???? ?????? ?????? ??.";
                return RedirectToAction("List");
            }

            return RedirectToAction("List");
        }
    }
}
