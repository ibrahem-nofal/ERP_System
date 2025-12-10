using ERP_System.Models;

namespace ERP_System.Services.Interfaces
{
    public interface ISupplierService
    {
        Task<List<Supplier>> GetAllAsync();
        Task<Supplier?> GetByIdAsync(int id);
        Task AddAsync(Supplier supplier, List<string> phones);
        Task UpdateAsync(Supplier supplier, List<string> phones);
        Task DeleteAsync(int id);
    }
}
