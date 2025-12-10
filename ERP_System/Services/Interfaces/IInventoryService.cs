using System.Threading.Tasks;

namespace ERP_System.Services.Interfaces
{
    public interface IInventoryService
    {
        Task AddStockAsync(int itemId, int storeId, int quantity, string notes, int? refId, string transactionType);
        Task RemoveStockAsync(int itemId, int storeId, int quantity, string notes, int? refId, string transactionType);
    }
}
