using ERP_System.ViewModels;

namespace ERP_System.Services.Interfaces
{
    public interface IReportService
    {
        Task<ReportsSalesVm> GetSalesSummaryAsync();
        Task<ReportsPurchasesVm> GetPurchasesSummaryAsync();
        Task<ReportsInventoryVm> GetInventorySummaryAsync();
    }
}
