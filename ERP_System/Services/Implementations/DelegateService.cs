using ERP_System.Data;
using ERP_System.Models;
using ERP_System.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ERP_System.Services.Implementations
{
    public class DelegateService : IDelegateService
    {
        private readonly AppDbContext _context;

        public DelegateService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<DelegateMember>> GetAllAsync()
        {
            return await _context.DelegateMembers
                .Include(d => d.Employee)
                    .ThenInclude(e => e.Phones)
                .OrderBy(d => d.Employee.Name)
                .ToListAsync();
        }

        public async Task<DelegateMember?> GetByEmployeeIdAsync(int empId)
        {
            return await _context.DelegateMembers
               .Include(d => d.Employee)
               .FirstOrDefaultAsync(m => m.EmpId == empId);
        }

        public async Task<DelegateMember?> GetByIdAsync(int id)
        {
            return await _context.DelegateMembers.FindAsync(id);
        }

        public async Task AddAsync(DelegateMember delegateMember)
        {
            _context.DelegateMembers.Add(delegateMember);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var delegateMember = await _context.DelegateMembers.FindAsync(id);
            if (delegateMember != null)
            {
                _context.DelegateMembers.Remove(delegateMember);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Employee>> GetEmployeesNotDelegatesAsync()
        {
            var existingDelegateIds = await _context.DelegateMembers.Select(d => d.EmpId).ToListAsync();
            return await _context.Employees
                .Where(e => !existingDelegateIds.Contains(e.Id))
                .ToListAsync();
        }

        public async Task<bool> IsEmployeeDelegateAsync(int empId)
        {
            return await _context.DelegateMembers.AnyAsync(d => d.EmpId == empId);
        }
    }
}
