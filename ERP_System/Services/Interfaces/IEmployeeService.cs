using ERP_System.Models;

namespace ERP_System.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task<bool> IsIdNumberExistsAsync(string idNumber, int? excludeId = null);
        Task AddAsync(Employee employee, List<string> phones, byte[]? imageData);
        Task UpdateAsync(Employee employee, List<string> phones, byte[]? imageData);
        Task DeleteAsync(int id);
        Task<EmpImage?> GetImageAsync(int empId);
    }
}
