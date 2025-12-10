using ERP_System.Data;
using ERP_System.Models;
using ERP_System.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ERP_System.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _context;

        public EmployeeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _context.Employees
                .Include(e => e.Phones)
                .Include(e => e.Image)
                .ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.Phones)
                .Include(e => e.Image)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<bool> IsIdNumberExistsAsync(string idNumber, int? excludeId = null)
        {
            if (excludeId.HasValue)
                return await _context.Employees.AnyAsync(e => e.IdNumber == idNumber && e.Id != excludeId.Value);
            return await _context.Employees.AnyAsync(e => e.IdNumber == idNumber);
        }

        public async Task<EmpImage?> GetImageAsync(int empId)
        {
            return await _context.EmpImages.FirstOrDefaultAsync(i => i.EmpId == empId);
        }

        public async Task AddAsync(Employee employee, List<string> phones, byte[]? imageData)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            if (phones != null)
            {
                foreach (var ph in phones.Where(p => !string.IsNullOrWhiteSpace(p)))
                {
                    _context.EmpPhones.Add(new EmpPhone
                    {
                        EmpId = employee.Id,
                        Phone = ph.Trim()
                    });
                }
            }

            if (imageData != null)
            {
                _context.EmpImages.Add(new EmpImage
                {
                    EmpId = employee.Id,
                    EmpImageData = imageData
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee employee, List<string> phones, byte[]? imageData)
        {
            var existing = await GetByIdAsync(employee.Id);
            if (existing == null) return;

            existing.Name = employee.Name;
            existing.RoleType = employee.RoleType;
            existing.Gender = employee.Gender;
            existing.Address = employee.Address;
            existing.IdNumber = employee.IdNumber;
            existing.BirthDate = employee.BirthDate;
            existing.Qualification = employee.Qualification;
            existing.State = employee.State;

            // Phones
            if (existing.Phones != null) _context.EmpPhones.RemoveRange(existing.Phones);
            if (phones != null)
            {
                foreach (var ph in phones.Where(p => !string.IsNullOrWhiteSpace(p)))
                {
                    _context.EmpPhones.Add(new EmpPhone { EmpId = existing.Id, Phone = ph.Trim() });
                }
            }

            // Image
            if (imageData != null)
            {
                if (existing.Image != null)
                {
                    existing.Image.EmpImageData = imageData;
                }
                else
                {
                    _context.EmpImages.Add(new EmpImage { EmpId = existing.Id, EmpImageData = imageData });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var employee = await GetByIdAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }
    }
}
