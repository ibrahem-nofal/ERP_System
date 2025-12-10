using ERP_System.Data;
using ERP_System.Models;
using ERP_System.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ERP_System.Services.Implementations
{
    public class ExpenseNameService : IExpenseNameService
    {
        private readonly AppDbContext _context;

        public ExpenseNameService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ExpenseName>> GetAllAsync()
        {
            return await _context.ExpenseNames.ToListAsync();
        }

        public async Task<ExpenseName?> GetByIdAsync(int id)
        {
            return await _context.ExpenseNames.FindAsync(id);
        }

        public async Task AddAsync(ExpenseName expenseName)
        {
            _context.ExpenseNames.Add(expenseName);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ExpenseName expenseName)
        {
            var existing = await _context.ExpenseNames.FindAsync(expenseName.Id);
            if (existing != null)
            {
                existing.Name = expenseName.Name;
                existing.Description = expenseName.Description;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.ExpenseNames.FindAsync(id);
            if (item != null)
            {
                _context.ExpenseNames.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
