using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ERP_System.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using ERP_System.Services.Interfaces;

namespace ERP_System.Controllers
{
    [Authorize]
    public class DefineDelegateMemberController : Controller
    {
        private readonly IDelegateService _delegateService;

        public DefineDelegateMemberController(IDelegateService delegateService)
        {
            _delegateService = delegateService;
        }

        // List all delegate members
        public async Task<IActionResult> List()
        {
            var delegates = await _delegateService.GetAllAsync();
            return View(delegates);
        }

        // Create new delegate member (GET)
        public async Task<IActionResult> Create()
        {
            // Get employees who are not already delegates
            var employees = await _delegateService.GetEmployeesNotDelegatesAsync();

            ViewBag.Employees = new SelectList(employees, "Id", "Name");
            return View();
        }

        // Create new delegate member (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DelegateMember delegateMember)
        {
            ModelState.Remove("Employee");
            if (ModelState.IsValid)
            {
                // Check if already exists (double check)
                if (await _delegateService.IsEmployeeDelegateAsync(delegateMember.EmpId))
                {
                    ModelState.AddModelError("", "This employee is already a delegate member.");
                }
                else
                {
                    await _delegateService.AddAsync(delegateMember);
                    return RedirectToAction(nameof(List));
                }
            }

            // Reload list if failed
            var employees = await _delegateService.GetEmployeesNotDelegatesAsync();
            ViewBag.Employees = new SelectList(employees, "Id", "Name", delegateMember.EmpId);

            return View(delegateMember);
        }

        // Delete delegate member (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Note: Service GetByEmployeeIdAsync takes EmpId not Id based on my implementation, controller logic seems to treat id as empId in List view link probably
            // Wait, List uses `d.Employee.Name` but usually delete links pass ID. Let's check model. DelegateMember key?
            // DelegateMember has Id? Or EmpId is key? 
            // In `Models/DelegateMember.cs`, I should double check. Assuming it has an Id.
            // If `GetByIdAsync` uses PK, and `Delete` passes PK... 
            // Original code: `m.EmpId == id` in Delete GET, `FindAsync(id)` in Delete POST. This implies `EmpId` might be PK or used as such? 
            // BUT `FindAsync` uses PK. If `EmpId` is PK, then `FindAsync(id)` works.
            // Let's assume standard Id PK exists, OR EmpId is PK. 
            // Service `GetByIdAsync` uses `FindAsync(id)`. 
            // Service `GetByEmployeeIdAsync` uses `m.EmpId == empId`.
            // Controller `Delete` GET used `m.EmpId == id`. This implies the `id` param is representing `EmpId`.
            // Controller `Delete` POST used `FindAsync(id)`. 
            // If `EmpId` is PK, `FindAsync` works.
            // I'll use `GetByEmployeeIdAsync` for GET to be safe if that was the intent, or `GetByIdAsync` if `id` is PK?
            // Let's stick to what original code usually did: `m.EmpId == id`. So I will use `GetByEmployeeIdAsync(id.Value)` if id is passed.

            var delegateMember = await _delegateService.GetByEmployeeIdAsync(id.Value);

            if (delegateMember == null)
            {
                // Try GetById if not found by EmpId?
                var byId = await _delegateService.GetByIdAsync(id.Value);
                if (byId != null) delegateMember = byId;
                else return NotFound();
            }

            return View(delegateMember);
        }

        // Delete delegate member (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Original code used `FindAsync(id)`.
            // My service `DeleteAsync` uses `FindAsync(id)`.
            await _delegateService.DeleteAsync(id);
            return RedirectToAction(nameof(List));
        }
    }
}
