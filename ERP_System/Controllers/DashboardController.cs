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

            // Monthly Sales & Purchases (Last 6 months)
            var monthlyStats = new List<dynamic>();
            var culture = new System.Globalization.CultureInfo("ar-EG");

            for (int i = 5; i >= 0; i--)
            {
                var date = DateTime.Now.AddMonths(-i);
                var monthStart = new DateTime(date.Year, date.Month, 1);
                var monthEnd = monthStart.AddMonths(1).AddDays(-1);

                var sales = await _context.InvoiceSaleHeaders
                    .Where(x => x.DateCreated >= monthStart && x.DateCreated <= monthEnd && (x.InvType == "SalesCash" || x.InvType == "SalesCredit"))
                    .SumAsync(x => (decimal?)x.NetAmount) ?? 0;

                var purchases = await _context.InvoicePurchaseHeaders
                    .Where(x => x.DateCreated >= monthStart && x.DateCreated <= monthEnd && (x.InvType == "PurchaseCash" || x.InvType == "PurchaseCredit"))
                    .SumAsync(x => (decimal?)x.NetAmount) ?? 0;

                monthlyStats.Add(new
                {
                    Month = date.ToString("MMMM", culture),
                    Sales = sales,
                    Purchases = purchases
                });
            }

            ViewBag.MonthlyStats = monthlyStats;

            return View();
        }
    }
}
