using ERP_System.Data;
using ERP_System.Models;
using ERP_System.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ERP_System.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _context;

        public AccountService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Login?> AuthenticateAsync(string username, string password)
        {
            return await _context.Logins
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            return await _context.Logins.AnyAsync(u => u.Username == username);
        }

        public async Task<bool> RegisterAsync(string username, string password)
        {
            if (await UserExistsAsync(username))
            {
                return false;
            }

            var newUser = new Login
            {
                Username = username,
                Password = password,
                IsActive = true
            };

            _context.Logins.Add(newUser);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
