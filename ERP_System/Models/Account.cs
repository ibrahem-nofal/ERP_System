using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        public int? ParentAccountId { get; set; }
        public string Type { get; set; }

        public Account ParentAccount { get; set; }
        public ICollection<Account> ChildAccounts { get; set; }
        public ICollection<Receipt> Receipts { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<Expense> Expenses { get; set; }
    }
}
