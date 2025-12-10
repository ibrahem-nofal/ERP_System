using ERP_System.Models;

namespace ERP_System.Services.Interfaces
{
    public interface IDelegateService
    {
        Task<List<DelegateMember>> GetAllAsync();
        Task<DelegateMember?> GetByEmployeeIdAsync(int empId);
        Task<DelegateMember?> GetByIdAsync(int id);
        Task AddAsync(DelegateMember delegateMember);
        Task DeleteAsync(int id);
        Task<List<Employee>> GetEmployeesNotDelegatesAsync();
        Task<bool> IsEmployeeDelegateAsync(int empId);
    }
}
