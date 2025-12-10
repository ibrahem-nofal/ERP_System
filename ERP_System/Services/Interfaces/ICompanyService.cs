using ERP_System.Models;

namespace ERP_System.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<List<Company>> GetAllAsync();
        Task<Company?> GetByIdAsync(int id);
        Task AddAsync(Company company, List<string> phones);
        Task UpdateAsync(Company company, List<string> phones);
        Task DeleteAsync(int id);
    }
}
