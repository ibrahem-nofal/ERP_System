using ERP_System.Models;

namespace ERP_System.ViewModels
{
    public class JournalEntryVm
    {
        public string Description { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public List<JournalDetailVm> Details { get; set; } = new List<JournalDetailVm>();
    }

    public class JournalDetailVm
    {
        public int AccountId { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string Note { get; set; }
    }
}
