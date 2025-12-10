using ERP_System.Models;

namespace ERP_System.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(int id);
        Task AddAsync(Customer customer, List<string> phones);
        Task UpdateAsync(Customer customer, List<string> phones);
        Task DeleteAsync(int id);
    }
}
