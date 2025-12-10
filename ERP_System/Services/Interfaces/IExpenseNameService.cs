using ERP_System.Models;

namespace ERP_System.Services.Interfaces
{
    public interface IExpenseNameService
    {
        Task<List<ExpenseName>> GetAllAsync();
        Task<ExpenseName?> GetByIdAsync(int id);
        Task AddAsync(ExpenseName expenseName);
        Task UpdateAsync(ExpenseName expenseName);
        Task DeleteAsync(int id);
    }
}
