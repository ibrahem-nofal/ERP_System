using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP_System.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly ERP_System.Services.Interfaces.IReportService _reportService;

        public ReportsController(ERP_System.Services.Interfaces.IReportService reportService)
        {
            _reportService = reportService;
        }

        public async Task<IActionResult> Sales()
        {
            var data = await _reportService.GetSalesSummaryAsync();
            return View(data);
        }

        public async Task<IActionResult> Purchases()
        {
            var data = await _reportService.GetPurchasesSummaryAsync();
            return View(data);
        }

        public async Task<IActionResult> Inventory()
        {
            var data = await _reportService.GetInventorySummaryAsync();
            return View(data);
        }
    }
}
