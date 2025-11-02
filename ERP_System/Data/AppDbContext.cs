using ERP_System.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ERP_System.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Users & Roles
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<Setting> Settings { get; set; }

        // Inventory
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<StockLevel> StockLevels { get; set; }
        public DbSet<StockTransfer> StockTransfers { get; set; }
        public DbSet<StockTransferItem> StockTransferItems { get; set; }

        // Sales
        public DbSet<Customer> Customers { get; set; }
        public DbSet<SalesInvoice> SalesInvoices { get; set; }
        public DbSet<SalesInvoiceItem> SalesInvoiceItems { get; set; }
        public DbSet<SalesReturn> SalesReturns { get; set; }
        public DbSet<SalesReturnItem> SalesReturnItems { get; set; }

        // Purchases
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }
        public DbSet<PurchaseInvoiceItem> PurchaseInvoiceItems { get; set; }
        public DbSet<PurchaseReturn> PurchaseReturns { get; set; }
        public DbSet<PurchaseReturnItem> PurchaseReturnItems { get; set; }

        // Accounting
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Expense> Expenses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== User & Roles =====
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            // ===== Self Referencing Account =====
            modelBuilder.Entity<Account>()
                .HasMany(a => a.ChildAccounts)
                .WithOne(a => a.ParentAccount)
                .HasForeignKey(a => a.ParentAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===== Product <-> Category =====
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===== StockLevel unique per (Product, Warehouse) =====
            modelBuilder.Entity<StockLevel>()
                .HasIndex(sl => new { sl.ProductId, sl.WarehouseId })
                .IsUnique();

            // ✅ StockTransfer Relationships (FromWarehouse / ToWarehouse)
            modelBuilder.Entity<StockTransfer>()
                .HasOne(st => st.FromWarehouse)
                .WithMany(w => w.FromTransfers)
                .HasForeignKey(st => st.FromWarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockTransfer>()
                .HasOne(st => st.ToWarehouse)
                .WithMany(w => w.ToTransfers)
                .HasForeignKey(st => st.ToWarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ StockTransferItem Relationship
            modelBuilder.Entity<StockTransferItem>()
                .HasOne(i => i.Transfer)
                .WithMany(t => t.Items)
                .HasForeignKey(i => i.TransferId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockTransferItem>()
                .HasOne(i => i.Product)
                .WithMany(p => p.StockTransferItems)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ StockLevel -> Product (Cascade Delete)
            modelBuilder.Entity<StockLevel>()
                .HasOne(s => s.Product)
                .WithMany(p => p.StockLevels)
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ ضبط Precision لجميع الخصائص من النوع decimal
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetPrecision(18);
                property.SetScale(2);
            }

            // ✅ جعل كل العلاقات DeleteBehavior.Restrict باستثناء Product → StockLevels
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                if (relationship.PrincipalEntityType.ClrType == typeof(Product) &&
                    relationship.DeclaringEntityType.ClrType == typeof(StockLevel))
                    continue; // استثناء العلاقة دي فقط

                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            // ✅ Category افتراضية علشان أي منتج يقدر يتسجل بدون مشكلة
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "افتراضي", Description = "الفئة الافتراضية للمنتجات" }
            );
        }
    }
}
