using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class Supplier
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم المورد مطلوب")]
        public string Name { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        public string Email { get; set; }

        public string? Address { get; set; }

        // ✅ اجعل Navigation Properties nullable أو = null!
        public ICollection<Payment>? Payments { get; set; }
        public ICollection<PurchaseInvoice>? PurchaseInvoices { get; set; }
    }
}
