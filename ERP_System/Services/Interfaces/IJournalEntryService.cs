using ERP_System.Models;
using ERP_System.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP_System.Services.Interfaces
{
    public interface IJournalEntryService
    {
        Task<List<JournalEntry>> GetAllAsync();
        Task<JournalEntry?> GetByIdAsync(int id);
        Task<int> AddAsync(JournalEntryVm vm, int? currentUserId = null);
        Task<List<ChartOfAccount>> GetLeafAccountsAsync();
        Task<int> GetOrCreateAccountAsync(string name, string code, string type);
        Task<int> CreateAutomaticEntryAsync(JournalEntry entry);
    }
}
