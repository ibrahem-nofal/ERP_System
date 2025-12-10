using ERP_System.Data;
using ERP_System.Models;
using ERP_System.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ERP_System.Services.Implementations
{
    public class SupplierService : ISupplierService
    {
        private readonly AppDbContext _context;

        public SupplierService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Supplier>> GetAllAsync()
        {
            return await _context.Suppliers.ToListAsync();
        }

        public async Task<Supplier?> GetByIdAsync(int id)
        {
            return await _context.Suppliers
                .Include(s => s.Phones)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddAsync(Supplier supplier, List<string> phones)
        {
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            if (phones != null)
            {
                foreach (var ph in phones.Where(p => !string.IsNullOrWhiteSpace(p)))
                {
                    _context.SupplierPhones.Add(new SupplierPhone
                    {
                        SupplierId = supplier.Id,
                        Phone = ph
                    });
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Supplier supplier, List<string> phones)
        {
            var existing = await GetByIdAsync(supplier.Id);
            if (existing == null) return;

            existing.Name = supplier.Name;
            existing.ManagerName = supplier.ManagerName;
            existing.ManagerPhone = supplier.ManagerPhone;
            existing.OwnerName = supplier.OwnerName;
            existing.OwnerPhone = supplier.OwnerPhone;
            existing.Address = supplier.Address;

            if (existing.Phones != null) _context.SupplierPhones.RemoveRange(existing.Phones);

            if (phones != null)
            {
                foreach (var ph in phones.Where(p => !string.IsNullOrWhiteSpace(p)))
                {
                    _context.SupplierPhones.Add(new SupplierPhone { SupplierId = existing.Id, Phone = ph });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var supplier = await GetByIdAsync(id);
            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
                await _context.SaveChangesAsync();
            }
        }
    }
}
