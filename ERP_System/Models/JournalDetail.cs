namespace ERP_System.Models
{
    public class JournalDetail
    {
        public int Id { get; set; }

        public int EntryId { get; set; }

        public int AccountId { get; set; }

        public decimal Debit { get; set; } = 0;

        public decimal Credit { get; set; } = 0;

        public string Note { get; set; }

   
        public JournalEntry Entry { get; set; }
        public ChartOfAccount Account { get; set; }
    }

}
