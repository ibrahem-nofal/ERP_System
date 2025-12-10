using ERP_System.Models;

namespace ERP_System.Services.Interfaces
{
    public interface IRevenueNameService
    {
        Task<List<RevenueName>> GetAllAsync();
        Task<RevenueName?> GetByIdAsync(int id);
        Task AddAsync(RevenueName revenueName);
        Task UpdateAsync(RevenueName revenueName);
        Task DeleteAsync(int id);
    }
}
