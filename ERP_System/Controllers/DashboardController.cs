using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ERP_System.Models;
using ERP_System.Data;
using Microsoft.EntityFrameworkCore;

namespace ERP_System.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get statistics
            var stats = new
            {
                TotalSales = await _context.InvoiceSaleHeaders
                    .Where(i => i.InvType == "SalesCash" || i.InvType == "SalesCredit")
                    .SumAsync(i => (decimal?)i.NetAmount) ?? 0,

                TotalPurchases = await _context.InvoicePurchaseHeaders
                    .Where(i => i.InvType == "PurchaseCash" || i.InvType == "PurchaseCredit")
                    .SumAsync(i => (decimal?)i.NetAmount) ?? 0,

                TotalCustomers = await _context.Customers.CountAsync(),

                TotalProducts = await _context.Items.CountAsync()
            };

            ViewBag.Stats = stats;

            // Get recent activities (last 5)
            var recentActivities = await _context.ActivityLogs
                .OrderByDescending(a => a.ActDate)
                .Take(5)
                .ToListAsync();

            ViewBag.RecentActivities = recentActivities;

            return View();
        }
    }
}
