using ERP_System.Data;
using ERP_System.Models;
using ERP_System.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ERP_System.Services.Implementations
{
    public class UnitService : IUnitService
    {
        private readonly AppDbContext _context;

        public UnitService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Unit>> GetAllAsync()
        {
            return await _context.Units.ToListAsync();
        }

        public async Task<Unit?> GetByIdAsync(int id)
        {
            return await _context.Units.FindAsync(id);
        }

        public async Task AddAsync(Unit unit)
        {
            _context.Units.Add(unit);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Unit unit)
        {
            var existing = await _context.Units.FindAsync(unit.Id);
            if (existing != null)
            {
                existing.Name = unit.Name;
                existing.Details = unit.Details;
                await _context.SaveChangesAsync();
            }
        }

        public async Task checkDelete(int id)
        {
            
        }

        public async Task DeleteAsync(int id)
        {
            var unit = await _context.Units.FindAsync(id);
            if (unit != null)
            {
                _context.Units.Remove(unit);
                await _context.SaveChangesAsync();
            }
        }
    }
}
