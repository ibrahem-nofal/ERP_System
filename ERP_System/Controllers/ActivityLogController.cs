using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP_System.Models;
using ERP_System.Data;

namespace ERP_System.Controllers
{
    [Authorize]
    public class ActivityLogController : Controller
    {
        private readonly AppDbContext _context;

        public ActivityLogController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> List()
        {
            var logs = await _context.ActivityLogs.OrderByDescending(l => l.ActDate).Take(100).ToListAsync();
            return View(logs);
        }
    }
}
