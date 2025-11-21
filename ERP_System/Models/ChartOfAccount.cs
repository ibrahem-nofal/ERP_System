namespace ERP_System.Models
{
    public class ChartOfAccount
    {
        public int Id { get; set; }

        public string Code { get; set; }          // Unique
        public string Name { get; set; }

        public string ParentCode { get; set; }    // References another account's Code

        public string AccountType { get; set; }   // Asset, Liability, Equity, Revenue, Expense

        public int Level { get; set; }            // 1–5

        public bool IsActive { get; set; } = true;

        public bool IsLeaf { get; set; } = false;

        public string NormalBalance { get; set; } // Debit, Credit

        // Optional hierarchical navigation
        public ChartOfAccount Parent { get; set; }
        public ICollection<ChartOfAccount> Children { get; set; }
    }

}
