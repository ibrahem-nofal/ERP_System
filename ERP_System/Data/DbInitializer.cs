using ERP_System.Models;
using System;
using System.Linq;

namespace ERP_System.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            // تأكد إن قاعدة البيانات موجودة
            context.Database.EnsureCreated();

            // ===== Roles =====
            if (!context.Roles.Any())
            {
                var roles = new Role[]
                {
                    new Role { RoleName = "Admin", Description = "Full system access and management" },
                    new Role { RoleName = "User", Description = "Limited access to basic modules" }
                };
                context.Roles.AddRange(roles);
                context.SaveChanges();
            }

            // ===== Default Admin User =====
            if (!context.Users.Any())
            {
                var admin = new User
                {
                    Username = "admin",
                    PasswordHash = "admin123", // ⚠️ لاحقاً استبدلها بـ Hashed Password
                    Email = "admin@erp.com",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                context.Users.Add(admin);
                context.SaveChanges();

                // Assign Admin Role
                var adminRole = context.Roles.First(r => r.RoleName == "Admin");
                context.UserRoles.Add(new UserRole
                {
                    UserId = admin.Id,
                    RoleId = adminRole.Id
                });
                context.SaveChanges();
            }

            // ===== Sample Categories =====
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category
                    {
                        Name = "Electronics",
                        Description = "Devices, gadgets, and electronic accessories."
                    },
                    new Category
                    {
                        Name = "Furniture",
                        Description = "Home and office furniture items."
                    }
                );
                context.SaveChanges();
            }

            // ===== Sample Warehouses =====
            if (!context.Warehouses.Any())
            {
                context.Warehouses.AddRange(
                    new Warehouse { Name = "Main Warehouse", Location = "Cairo HQ" },
                    new Warehouse { Name = "Branch Warehouse", Location = "Alexandria" }
                );
                context.SaveChanges();
            }

            // ===== Default Settings =====
            if (!context.Settings.Any())
            {
                context.Settings.Add(new Setting
                {
                    CompanyName = "My ERP System",
                    LogoPath = "/images/logo.png",
                    Language = "en"
                });
                context.SaveChanges();
            }
        }
    }
}
