using ERP_System.Data;
using ERP_System.Models;
using ERP_System.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ERP_System.Services.Implementations
{
    public class CompanyService : ICompanyService
    {
        private readonly AppDbContext _context;

        public CompanyService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Company>> GetAllAsync()
        {
            return await _context.Companies.ToListAsync();
        }

        public async Task<Company?> GetByIdAsync(int id)
        {
            return await _context.Companies
                .Include(c => c.Phones)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Company company, List<string> phones)
        {
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            if (phones != null && phones.Any())
            {
                foreach (var ph in phones)
                {
                    if (!string.IsNullOrWhiteSpace(ph))
                    {
                        var compPhone = new CompanyPhone
                        {
                            CompId = company.Id, // Ensure ID is linked (EF usually handles object link but explicit ID is safe)
                            Company = company,
                            Phone = ph
                        };
                        _context.CompanyPhones.Add(compPhone);
                    }
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Company company, List<string> phones)
        {
            var existingCompany = await GetByIdAsync(company.Id);
            if (existingCompany == null) return;

            // Update properties
            existingCompany.Code = company.Code;
            existingCompany.Name = company.Name;
            existingCompany.Address = company.Address;
            existingCompany.DateCreated = company.DateCreated;
            existingCompany.OtherDetails = company.OtherDetails;

            // Update phones
            _context.CompanyPhones.RemoveRange(existingCompany.Phones);

            if (phones != null)
            {
                foreach (var ph in phones)
                {
                    if (!string.IsNullOrWhiteSpace(ph))
                    {
                        _context.CompanyPhones.Add(new CompanyPhone { Company = existingCompany, Phone = ph });
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company != null)
            {
                _context.Companies.Remove(company);
                await _context.SaveChangesAsync();
            }
        }
    }
}
