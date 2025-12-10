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
            
            await _delegateService.DeleteAsync(id);
            return RedirectToAction(nameof(List));
        }
    }
}
