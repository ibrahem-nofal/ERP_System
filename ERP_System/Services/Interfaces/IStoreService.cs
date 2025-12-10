using ERP_System.Models;

namespace ERP_System.Services.Interfaces
{
    public interface IStoreService
    {
        Task<List<Store>> GetAllAsync();
        Task<Store?> GetByIdAsync(int id);
        Task<bool> IsNameExistsAsync(string name, int? excludeId = null);
        Task AddAsync(Store store, List<string> phones, byte[]? imageData);
        Task UpdateAsync(Store store, List<string> phones, byte[]? imageData);
        Task DeleteAsync(int id);
    }
}
