using Microsoft.EntityFrameworkCore;
using ERP_System.Models;

namespace ERP_System.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<EmpPhone> EmpPhones { get; set; }
        public DbSet<EmpImage> EmpImages { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyPhone> CompanyPhones { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<StorePhone> StorePhones { get; set; }
        public DbSet<StoreImage> StoreImages { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemCode> ItemCodes { get; set; }
        public DbSet<ItemImage> ItemImages { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierPhone> SupplierPhones { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerPhone> CustomerPhones { get; set; }
        public DbSet<ExpenseName> ExpenseNames { get; set; }
        public DbSet<RevenueName> RevenueNames { get; set; }

        public DbSet<InvoicePurchaseHeader> InvoicePurchaseHeaders { get; set; }
        public DbSet<InvoicePurchaseDetail> InvoicePurchaseDetails { get; set; }
        public DbSet<PurchasePayment> PurchasePayments { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
        public DbSet<ChartOfAccount> ChartOfAccounts { get; set; }
        public DbSet<JournalEntry> JournalEntries { get; set; }
        public DbSet<JournalDetail> JournalDetails { get; set; }
        public DbSet<DelegateMember> DelegateMembers { get; set; }
        public DbSet<CustomerItem> CustomerItems { get; set; }
        public DbSet<InvoiceSaleHeader> InvoiceSaleHeaders { get; set; }
        public DbSet<InvoiceSaleDetail> InvoiceSaleDetails { get; set; }
        public DbSet<SalePayment> SalePayments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanyPhone>()
                .HasKey(cp => new { cp.CompId, cp.Phone });

            modelBuilder.Entity<EmpPhone>()
                .HasKey(ep => new { ep.EmpId, ep.Phone });

            modelBuilder.Entity<ItemCategory>()
                .HasKey(ic => new { ic.ItemId, ic.CategoryId });

            modelBuilder.Entity<ItemCode>()
                .HasKey(ic => new { ic.ItemId, ic.ItemCodeValue });

            modelBuilder.Entity<CustomerPhone>()
                .HasKey(cp => new { cp.CustomerId, cp.Phone });

            modelBuilder.Entity<SupplierPhone>()
                .HasKey(sp => new { sp.SupplierId, sp.Phone });



            // ibrahem 
            modelBuilder.Entity<InvoicePurchaseHeader>()
                .Property(p => p.NetAmount)
                .HasComputedColumnSql("[totalAmount] - [discount]", stored: true);

            modelBuilder.Entity<InvoicePurchaseHeader>()
                .HasOne(p => p.RefInvoice)
                .WithMany()
                .HasForeignKey(p => p.RefInvId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InvoicePurchaseDetail>()
                .Property(p => p.TotalPrice)
                .HasComputedColumnSql("[quantity] * [unitPrice]", stored: true);



            modelBuilder.Entity<InvoicePurchaseDetail>()
                .HasOne(d => d.Invoice)
                .WithMany()
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InvoicePurchaseDetail>()
                .HasOne(d => d.ReturnedInvoice)
                .WithMany()
                .HasForeignKey(d => d.InvReturnId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PurchasePayment>()
                .HasOne(p => p.InvoicePurchase)
                .WithMany()
                .HasForeignKey(p => p.InvPurchaseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PurchasePayment>()
                .HasOne(p => p.InvoiceReturn)
                .WithMany()
                .HasForeignKey(p => p.InvoiceReturnId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Inventory>()
                .HasKey(i => new { i.StoreId, i.ItemId });

            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Store)
                .WithMany()
                .HasForeignKey(i => i.StoreId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Item)
                .WithMany()
                .HasForeignKey(i => i.ItemId)
                .OnDelete(DeleteBehavior.Restrict);




            modelBuilder.Entity<InventoryTransaction>()
                .HasOne(t => t.Item)
                .WithMany()
                .HasForeignKey(t => t.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InventoryTransaction>()
                .HasOne(t => t.Store)
                .WithMany()
                .HasForeignKey(t => t.StoreId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InventoryTransaction>()
                .HasOne(t => t.AssignedByEmployee)
                .WithMany()
                .HasForeignKey(t => t.AssignedBy)
                .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<ChartOfAccount>()
                .HasIndex(a => a.Code)
                .IsUnique();

            modelBuilder.Entity<ChartOfAccount>()
                .HasOne(a => a.Parent)
                .WithMany(a => a.Children)
                .HasForeignKey(a => a.ParentCode)
                .HasPrincipalKey(a => a.Code)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JournalEntry>()
                .HasOne(e => e.InvoicePurchase)
                .WithMany()
                .HasForeignKey(e => e.InvPurId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JournalEntry>()
                .HasOne(e => e.InvoiceSale)
                .WithMany()
                .HasForeignKey(e => e.InvSaleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JournalEntry>()
                .HasOne(e => e.AssignedByEmployee)
                .WithMany()
                .HasForeignKey(e => e.AssignedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JournalEntry>()
                .HasMany(e => e.Details)
                .WithOne(d => d.Entry)
                .HasForeignKey(d => d.EntryId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<JournalDetail>()
                .HasOne(d => d.Account)
                .WithMany()
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            // DelegateMember
            modelBuilder.Entity<DelegateMember>()
                .HasKey(d => d.EmpId);

            // CustomerItem
            modelBuilder.Entity<CustomerItem>()
                .HasKey(ci => new { ci.CustomerId, ci.ItemId });

            // InvoiceSaleHeader
            modelBuilder.Entity<InvoiceSaleHeader>()
                .Property(p => p.NetAmount)
                .HasComputedColumnSql("[totalAmount] - [discount]", stored: true);

            modelBuilder.Entity<InvoiceSaleHeader>()
                .HasOne(s => s.RefInvoice)
                .WithMany()
                .HasForeignKey(s => s.RefInvId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InvoiceSaleHeader>()
                .HasOne(s => s.Customer)
                .WithMany()
                .HasForeignKey(s => s.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InvoiceSaleHeader>()
                .HasOne(s => s.Delegate)
                .WithMany()
                .HasForeignKey(s => s.DelegateId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InvoiceSaleHeader>()
                .HasOne(s => s.Store)
                .WithMany()
                .HasForeignKey(s => s.StoreId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InvoiceSaleHeader>()
                .HasOne(s => s.AssignedByEmployee)
                .WithMany()
                .HasForeignKey(s => s.AssignedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // InvoiceSaleDetail
            modelBuilder.Entity<InvoiceSaleDetail>()
                .Property(p => p.TotalPrice)
                .HasComputedColumnSql("[quantity] * [unitPrice]", stored: true);

            modelBuilder.Entity<InvoiceSaleDetail>()
                .HasOne(d => d.Invoice)
                .WithMany()
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InvoiceSaleDetail>()
                .HasOne(d => d.InvReturn)
                .WithMany()
                .HasForeignKey(d => d.InvReturnId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InvoiceSaleDetail>()
                .HasOne(d => d.Item)
                .WithMany()
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // SalePayment
            modelBuilder.Entity<SalePayment>()
                .HasOne(p => p.InvoiceSale)
                .WithMany()
                .HasForeignKey(p => p.InvSaleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SalePayment>()
                .HasOne(p => p.InvoiceReturn)
                .WithMany()
                .HasForeignKey(p => p.InvoiceReturnId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SalePayment>()
                .HasOne(p => p.InvoiceReturn)
                .WithMany()
                .HasForeignKey(p => p.InvoiceReturnId)
                .OnDelete(DeleteBehavior.Restrict);

            // Unique Indexes
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.IdNumber)
                .IsUnique();

            modelBuilder.Entity<ItemCode>()
                .HasIndex(ic => ic.ItemCodeValue)
                .IsUnique();

            // Check Constraints
            modelBuilder.Entity<Employee>()
                .ToTable(t => t.HasCheckConstraint("CK_Employee_RoleType", "[RoleType] IN ('delegate', 'emp')"));

            modelBuilder.Entity<InvoicePurchaseHeader>()
                .ToTable(t =>
                {
                    t.HasCheckConstraint("CK_InvoicePurchaseHeader_InvType", "[InvType] IN ('PurchaseCash', 'PurchaseCredit', 'PurchaseReturn')");
                    t.HasCheckConstraint("CK_InvoicePurchaseHeader_OrderStatus", "[OrderStatus] IN ('wait', 'recieved', 'returned', 'returning')");
                    t.HasCheckConstraint("CK_InvoicePurchaseHeader_PayStatus", "[PayStatus] IN ('open', 'closed')");
                });

            modelBuilder.Entity<InvoicePurchaseDetail>()
                .ToTable(t => t.HasCheckConstraint("CK_InvoicePurchaseDetail_Status", "[Status] IN ('Purchased', 'Returned')"));

            modelBuilder.Entity<PurchasePayment>()
                .ToTable(t => t.HasCheckConstraint("CK_PurchasePayment_PaymentMethod", "[PaymentMethod] IN ('cash', 'visa', 'vCash', 'insta')"));

            modelBuilder.Entity<InvoiceSaleHeader>()
                .ToTable(t =>
                {
                    t.HasCheckConstraint("CK_InvoiceSaleHeader_InvType", "[InvType] IN ('SalesCash', 'SalesReturnCash', 'SalesCredit', 'SalesReturnCredit')");
                    t.HasCheckConstraint("CK_InvoiceSaleHeader_OrderStatus", "[OrderStatus] IN ('wait', 'sent', 'returned', 'returning')");
                    t.HasCheckConstraint("CK_InvoiceSaleHeader_PayStatus", "[PayStatus] IN ('open', 'closed')");
                });

            modelBuilder.Entity<InvoiceSaleDetail>()
                .ToTable(t => t.HasCheckConstraint("CK_InvoiceSaleDetail_Status", "[Status] IN ('Purchased', 'Returned')"));

            modelBuilder.Entity<SalePayment>()
                .ToTable(t => t.HasCheckConstraint("CK_SalePayment_PaymentMethod", "[PaymentMethod] IN ('cash', 'visa', 'vCash', 'insta')"));

            modelBuilder.Entity<InventoryTransaction>()
                .ToTable(t => t.HasCheckConstraint("CK_InventoryTransaction_TransactionType", "[TransactionType] IN ('Purchase', 'PurchaseReturn', 'Sales', 'SalesReturn', 'Adjustment')"));

            modelBuilder.Entity<ChartOfAccount>()
                .ToTable(t =>
                {
                    t.HasCheckConstraint("CK_ChartOfAccount_AccountType", "[AccountType] IN ('Asset', 'Liability', 'Equity', 'Revenue', 'Expense')");
                    t.HasCheckConstraint("CK_ChartOfAccount_Level", "[Level] BETWEEN 1 AND 5");
                    t.HasCheckConstraint("CK_ChartOfAccount_NormalBalance", "[NormalBalance] IN ('Debit', 'Credit')");
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
