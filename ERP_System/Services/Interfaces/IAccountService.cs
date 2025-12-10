using ERP_System.Models;

namespace ERP_System.Services.Interfaces
{
    public interface IAccountService
    {
        Task<Login?> AuthenticateAsync(string username, string password);
        Task<bool> RegisterAsync(string username, string password);
        Task<bool> UserExistsAsync(string username);
    }
}
