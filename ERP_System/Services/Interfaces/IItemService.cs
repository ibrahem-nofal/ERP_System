using ERP_System.Models;

namespace ERP_System.Services.Interfaces
{
    public interface IItemService
    {
        Task<List<Item>> GetAllAsync();
        Task<Item?> GetByIdAsync(int id);
        Task<List<int>> GetCategoryIdsAsync(int itemId);
        Task AddAsync(Item item, List<string> codes, List<int> categoryIds, byte[]? imageData);
        Task UpdateAsync(Item item, List<string> codes, List<int> categoryIds, byte[]? imageData);
        Task DeleteAsync(int id);
    }
}
