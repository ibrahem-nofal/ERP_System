using ERP_System.Data;
using ERP_System.Models;
using ERP_System.Services.Interfaces;
using ERP_System.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP_System.Services.Implementations
{
    public class JournalEntryService : IJournalEntryService
    {
        private readonly AppDbContext _context;

        public JournalEntryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<JournalEntry>> GetAllAsync()
        {
            return await _context.JournalEntries
                .Include(j => j.AssignedByEmployee)
                .OrderByDescending(j => j.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<JournalEntry?> GetByIdAsync(int id)
        {
            return await _context.JournalEntries
                .Include(j => j.Details)
                .ThenInclude(d => d.Account)
                .Include(j => j.AssignedByEmployee)
                .FirstOrDefaultAsync(j => j.Id == id);
        }

        public async Task<int> AddAsync(JournalEntryVm vm, int? currentUserId = null)
        {
            var entry = new JournalEntry
            {
                Description = vm.Description,
                CreatedAt = vm.Date,
                SourceType = "Manual",
                AssignedBy = currentUserId,
                Details = vm.Details.Select(d => new JournalDetail
                {
                    AccountId = d.AccountId,
                    Debit = d.Debit,
                    Credit = d.Credit,
                    Note = d.Note
                }).ToList()
            };

            _context.JournalEntries.Add(entry);
            await _context.SaveChangesAsync();
            return entry.Id;
        }

        public async Task<List<ChartOfAccount>> GetLeafAccountsAsync()
        {
            return await _context.ChartOfAccounts
                .Where(a => a.IsLeaf)
                .OrderBy(a => a.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<int> GetOrCreateAccountAsync(string name, string code, string type)
        {
            var account = await _context.ChartOfAccounts.FirstOrDefaultAsync(a => a.Code == code);
            if (account == null)
            {
                account = new ChartOfAccount
                {
                    Name = name,
                    Code = code,
                    AccountType = type,
                    IsLeaf = true,
                    Level = 1,
                    IsActive = true
                };
                _context.ChartOfAccounts.Add(account);
                await _context.SaveChangesAsync();
            }
            return account.Id;
        }

        public async Task<int> CreateAutomaticEntryAsync(JournalEntry entry)
        {
            _context.JournalEntries.Add(entry);
            await _context.SaveChangesAsync();
            return entry.Id;
        }
    }
}
