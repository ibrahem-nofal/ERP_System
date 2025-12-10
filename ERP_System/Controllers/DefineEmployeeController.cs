using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ERP_System.Models;
using ERP_System.ViewModels;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ERP_System.Services.Interfaces;

namespace ERP_System.Controllers
{
    [Authorize]
    public class DefineEmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public DefineEmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public async Task<IActionResult> List()
        {
            var employees = await _employeeService.GetAllAsync();
            return View(employees);
        }

        public IActionResult Index()
        {
            return View(new AddEmpVm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(AddEmpVm advm)
        {
            
            advm.IdNumber = advm.IdNumber?.Trim();
            advm.Name = advm.Name?.Trim();
            advm.Address = advm.Address?.Trim();

            
            if (string.IsNullOrWhiteSpace(advm.Name))
                ModelState.AddModelError(nameof(advm.Name), "?????? ????? ??? ??????.");

            
            if (string.IsNullOrWhiteSpace(advm.IdNumber))
                ModelState.AddModelError(nameof(advm.IdNumber), "?????? ????? ??? ??????.");

            
            if (!string.IsNullOrWhiteSpace(advm.IdNumber) &&
                await _employeeService.IsIdNumberExistsAsync(advm.IdNumber))
            {
                ModelState.AddModelError(nameof(advm.IdNumber), "??? ?????? ??? ?????? ?? ???? ?????? ????? ??? ???.");
            }

            if (!ModelState.IsValid)
            {
                return View(advm);
            }

            byte[]? imageBytes = null;
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
                await _employeeService.AddAsync(emp, advm.Phones, imageBytes);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "????? ??? ????????. ?? ???? ???? ??? ???? ???? ?? ????? ?? ????? ????????.");
                return View(advm);
            }

            return RedirectToAction("List");
        }

        
        public async Task<IActionResult> Details(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AddEmpVm advm)
        {
            advm.IdNumber = advm.IdNumber?.Trim();
            advm.Name = advm.Name?.Trim();
            advm.Address = advm.Address?.Trim();

           
            if (string.IsNullOrWhiteSpace(advm.Name))
                ModelState.AddModelError(nameof(advm.Name), "?????? ????? ??? ??????.");

            if (string.IsNullOrWhiteSpace(advm.IdNumber))
                ModelState.AddModelError(nameof(advm.IdNumber), "?????? ????? ??? ??????.");

            
            if (!string.IsNullOrWhiteSpace(advm.IdNumber) &&
                await _employeeService.IsIdNumberExistsAsync(advm.IdNumber, id))
            {
                ModelState.AddModelError(nameof(advm.IdNumber), "رقم الهوية هذا مسجل من قبل، يرجى التحقق من الرقم.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.EmployeeId = id;
                return View(advm);
            }

            
            var employee = new Employee
            {
                Id = id,
                Name = advm.Name,
                RoleType = ((RoleType)advm.RoleType).ToString(),
                Gender = ((Gender)advm.Gender).ToString(),
                Address = advm.Address,
                IdNumber = advm.IdNumber,
                BirthDate = advm.BirthDate,
                Qualification = ((Qualification)advm.Qualification).ToString(),
                State = ((State)advm.State).ToString()
            };

            
            byte[]? imageBytes = null;
            if (advm.EmpImage != null)
            {
                using (var ms = new MemoryStream())
                {
                    await advm.EmpImage.CopyToAsync(ms);
                    imageBytes = ms.ToArray();
                }
            }

            try
            {
                await _employeeService.UpdateAsync(employee, advm.Phones, imageBytes);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "????? ??? ?????????. ?? ???? ???? ??? ???? ???? ?? ????? ?? ????? ????????.");
                ViewBag.EmployeeId = id;
                return View(advm);
            }

            return RedirectToAction("List");
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _employeeService.DeleteAsync(id);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "?? ???? ??? ?????? ???? ???? ?????? ?????? ??.";
                return RedirectToAction("List");
            }

            return RedirectToAction("List");
        }

        // Action لعرض الصورة
        [HttpGet]
        public async Task<IActionResult> GetImage(int id)
        {
            var empImage = await _employeeService.GetImageAsync(id);
            if (empImage != null && empImage.EmpImageData != null)
            {
                return File(empImage.EmpImageData, "image/jpeg"); // أو نوع الصورة المناسب
            }
            return NotFound();
        }
    }
}
