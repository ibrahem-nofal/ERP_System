using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System.Models
{
    public class InvoiceTransferHeader
    {
        [Key]
        public int Id { get; set; }

        public string? Code { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        public int FromStoreId { get; set; }

        [Required]
        public int ToStoreId { get; set; }

        public string? Remarks { get; set; }

        public int? AssignedBy { get; set; }

        [ForeignKey("FromStoreId")]
        public virtual Store? FromStore { get; set; }

        [ForeignKey("ToStoreId")]
        public virtual Store? ToStore { get; set; }

        [ForeignKey("AssignedBy")]
        public virtual Employee? AssignedByEmployee { get; set; }

        public virtual ICollection<InvoiceTransferDetail> Details { get; set; } = new List<InvoiceTransferDetail>();
    }
}
