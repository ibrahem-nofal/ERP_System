using ERP_System.Data;
using ERP_System.Models;
using ERP_System.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ERP_System.Services.Implementations
{
    public class RevenueNameService : IRevenueNameService
    {
        private readonly AppDbContext _context;

        public RevenueNameService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<RevenueName>> GetAllAsync()
        {
            return await _context.RevenueNames.ToListAsync();
        }

        public async Task<RevenueName?> GetByIdAsync(int id)
        {
            return await _context.RevenueNames.FindAsync(id);
        }

        public async Task AddAsync(RevenueName revenueName)
        {
            _context.RevenueNames.Add(revenueName);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RevenueName revenueName)
        {
            var existing = await _context.RevenueNames.FindAsync(revenueName.Id);
            if (existing != null)
            {
                existing.Name = revenueName.Name;
                existing.Description = revenueName.Description;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.RevenueNames.FindAsync(id);
            if (item != null)
            {
                _context.RevenueNames.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
