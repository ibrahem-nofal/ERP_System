using ERP_System.Data;
using ERP_System.Models;
using ERP_System.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ERP_System.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _context;

        public CustomerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _context.Customers
                .Include(c => c.Phones)
                .ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers
                .Include(c => c.Phones)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Customer customer, List<string> phones)
        {
            _context.Customers.Add(customer);
            // Save to get ID
            await _context.SaveChangesAsync();

            if (phones != null && phones.Any())
            {
                foreach (var ph in phones)
                {
                    _context.CustomerPhones.Add(new CustomerPhone
                    {
                        CustomerId = customer.Id,
                        Phone = ph
                    });
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Customer customer, List<string> phones)
        {
            var existing = await GetByIdAsync(customer.Id);
            if (existing == null) return;

            existing.Name = customer.Name;
            existing.Gender = customer.Gender;
            existing.Address = customer.Address;
            existing.StartDate = customer.StartDate;
            existing.BirthDate = customer.BirthDate;
            existing.OtherDetails = customer.OtherDetails;

            // Phones
            if (existing.Phones != null) _context.CustomerPhones.RemoveRange(existing.Phones);

            if (phones != null)
            {
                foreach (var ph in phones)
                {
                    _context.CustomerPhones.Add(new CustomerPhone { CustomerId = existing.Id, Phone = ph });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var customer = await GetByIdAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }
    }
}
