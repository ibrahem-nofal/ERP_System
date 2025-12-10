using ERP_System.Data;
using ERP_System.Services.Interfaces;
using ERP_System.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ERP_System.Services.Implementations
{
    public class ReportService : IReportService
    {
        private readonly AppDbContext _context;

        public ReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ReportsSalesVm> GetSalesSummaryAsync()
        {
            var now = DateTime.Now;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var startOfLastMonth = startOfMonth.AddMonths(-1);
            var endOfLastMonth = startOfMonth.AddDays(-1);

            // Fetch Headers for calculations
            var salesThisMonth = await _context.InvoiceSaleHeaders
                .Where(x => x.DateCreated >= startOfMonth)
                .ToListAsync();

            var salesLastMonth = await _context.InvoiceSaleHeaders
                .Where(x => x.DateCreated >= startOfLastMonth && x.DateCreated <= endOfLastMonth)
                .ToListAsync();

            var allSales = await _context.InvoiceSaleHeaders.ToListAsync();

            // Basic Metrics
            decimal totalSales = allSales.Sum(x => x.TotalAmount);
            int count = allSales.Count;
            decimal avg = count > 0 ? totalSales / count : 0;

            // Comparison
            decimal thisMonthTotal = salesThisMonth.Sum(x => x.TotalAmount);
            decimal lastMonthTotal = salesLastMonth.Sum(x => x.TotalAmount);
            decimal percentage = 0;
            if (lastMonthTotal > 0)
            {
                percentage = ((thisMonthTotal - lastMonthTotal) / lastMonthTotal) * 100;
            }
            else if (thisMonthTotal > 0)
            {
                percentage = 100;
            }

            // New Customers this month
            var newCustCount = await _context.Customers
                .Where(c => c.StartDate >= startOfMonth)
                .CountAsync();

            // Monthly Sales (Last 6 months)
            var monthlyData = new Dictionary<string, decimal>();
            for (int i = 5; i >= 0; i--)
            {
                var monthDate = now.AddMonths(-i);
                var monthStart = new DateTime(monthDate.Year, monthDate.Month, 1);
                var monthEnd = monthStart.AddMonths(1).AddDays(-1);

                var sum = await _context.InvoiceSaleHeaders
                    .Where(x => x.DateCreated >= monthStart && x.DateCreated <= monthEnd)
                    .SumAsync(x => x.TotalAmount);

                monthlyData.Add(monthDate.ToString("MMM"), sum);
            }

            // Top Selling Items
            var topItems = await _context.InvoiceSaleDetails
               .Include(d => d.Item)
               .Where(d => d.Item != null)
               .GroupBy(d => d.Item.Name)
               .Select(g => new { Name = g.Key, Qty = g.Sum(x => x.Quantity) })
               .OrderByDescending(x => x.Qty)
               .Take(5)
               .ToDictionaryAsync(x => x.Name, x => x.Qty);


            return new ReportsSalesVm
            {
                TotalSales = totalSales,
                InvoiceCount = count,
                AverageInvoiceValue = avg,
                NewCustomersCount = newCustCount,
                TotalSalesComparisonPercentage = Math.Round(percentage, 1),
                MonthlySales = monthlyData,
                TopSellingItems = topItems
            };
        }

        public async Task<ReportsPurchasesVm> GetPurchasesSummaryAsync()
        {
            var now = DateTime.Now;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var startOfLastMonth = startOfMonth.AddMonths(-1);
            var endOfLastMonth = startOfMonth.AddDays(-1);

            var purchasesThisMonth = await _context.InvoicePurchaseHeaders
               .Where(x => x.DateCreated >= startOfMonth)
               .SumAsync(x => x.TotalAmount);

            var purchasesLastMonth = await _context.InvoicePurchaseHeaders
                .Where(x => x.DateCreated >= startOfLastMonth && x.DateCreated <= endOfLastMonth)
                .SumAsync(x => x.TotalAmount);

            var allPurchases = await _context.InvoicePurchaseHeaders.ToListAsync();
            decimal totalPurchases = allPurchases.Sum(x => x.TotalAmount);

            decimal percentage = 0;
            if (purchasesLastMonth > 0)
            {
                percentage = ((purchasesThisMonth - purchasesLastMonth) / purchasesLastMonth) * 100;
            }
            else if (purchasesThisMonth > 0) percentage = 100;

            // Total Payments
            var totalPayments = await _context.PurchasePayments.SumAsync(x => x.AmountPaid);

            
            int newSuppliers = 0;

            // Monthly Purchases
            var monthlyData = new Dictionary<string, decimal>();
            for (int i = 5; i >= 0; i--)
            {
                var monthDate = now.AddMonths(-i);
                var monthStart = new DateTime(monthDate.Year, monthDate.Month, 1);
                var monthEnd = monthStart.AddMonths(1).AddDays(-1);

                var sum = await _context.InvoicePurchaseHeaders
                    .Where(x => x.DateCreated >= monthStart && x.DateCreated <= monthEnd)
                    .SumAsync(x => x.TotalAmount);

                monthlyData.Add(monthDate.ToString("MMM"), sum);
            }

            // Top Suppliers
            var topSuppliers = await _context.InvoicePurchaseHeaders
                .Include(h => h.Supplier)
                .Where(h => h.Supplier != null)
                .GroupBy(h => h.Supplier.Name)
                .Select(g => new { Name = g.Key, Total = g.Sum(x => x.TotalAmount) })
                .OrderByDescending(x => x.Total)
                .Take(5)
                .ToDictionaryAsync(x => x.Name, x => x.Total);

            return new ReportsPurchasesVm
            {
                TotalPurchases = totalPurchases,
                InvoiceCount = allPurchases.Count,
                NewSuppliersCount = newSuppliers,
                TotalPayments = totalPayments,
                TotalPurchasesComparisonPercentage = Math.Round(percentage, 1),
                MonthlyPurchases = monthlyData,
                TopSuppliers = topSuppliers
            };
        }

        public async Task<ReportsInventoryVm> GetInventorySummaryAsync()
        {
            var inventoryValue = await _context.Inventories
                .Include(i => i.Item)
                .SumAsync(x => x.CurrentQuantity * (x.Item.BuyPrice ?? 0));

            var totalItems = await _context.Items.CountAsync();

            var lowStock = await _context.Inventories
               .Where(x => x.CurrentQuantity < 10)
               .CountAsync();

            // Inventory Movements Today
            var today = DateTime.Today;
            var movements = await _context.InventoryTransactions
                .Where(t => t.TransactionDate >= today)
                .CountAsync();

           
            var byCategory = await _context.ItemCategories
                .Include(ic => ic.Category)
                .GroupBy(ic => ic.Category.Name)
                .Select(g => new { Name = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Name, x => x.Count);

            return new ReportsInventoryVm
            {
                TotalInventoryValue = inventoryValue,
                TotalItemsCount = totalItems,
                LowStockItemsCount = lowStock,
                InventoryMovementsCount = movements,
                InventoryByCategory = byCategory
            };
        }
    }
}
