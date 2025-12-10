using Microsoft.EntityFrameworkCore;
using ERP_System;
using ERP_System.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ERP_System.Services.Interfaces.ICompanyService, ERP_System.Services.Implementations.CompanyService>();
builder.Services.AddScoped<ERP_System.Services.Interfaces.IStoreService, ERP_System.Services.Implementations.StoreService>();
builder.Services.AddScoped<ERP_System.Services.Interfaces.IUnitService, ERP_System.Services.Implementations.UnitService>();
builder.Services.AddScoped<ERP_System.Services.Interfaces.IItemService, ERP_System.Services.Implementations.ItemService>();
builder.Services.AddScoped<ERP_System.Services.Interfaces.ICategoryService, ERP_System.Services.Implementations.CategoryService>();
builder.Services.AddScoped<ERP_System.Services.Interfaces.IAccountService, ERP_System.Services.Implementations.AccountService>();
builder.Services.AddScoped<ERP_System.Services.Interfaces.ICustomerService, ERP_System.Services.Implementations.CustomerService>();
builder.Services.AddScoped<ERP_System.Services.Interfaces.ISupplierService, ERP_System.Services.Implementations.SupplierService>();
builder.Services.AddScoped<ERP_System.Services.Interfaces.IEmployeeService, ERP_System.Services.Implementations.EmployeeService>();
builder.Services.AddScoped<ERP_System.Services.Interfaces.IDelegateService, ERP_System.Services.Implementations.DelegateService>();
builder.Services.AddScoped<ERP_System.Services.Interfaces.IExpenseNameService, ERP_System.Services.Implementations.ExpenseNameService>();
builder.Services.AddScoped<ERP_System.Services.Interfaces.IRevenueNameService, ERP_System.Services.Implementations.RevenueNameService>();
builder.Services.AddScoped<ERP_System.Services.Interfaces.IReportService, ERP_System.Services.Implementations.ReportService>();
builder.Services.AddScoped<ERP_System.Services.Interfaces.IInventoryService, ERP_System.Services.Implementations.InventoryService>();
builder.Services.AddScoped<ERP_System.Services.Interfaces.IJournalEntryService, ERP_System.Services.Implementations.JournalEntryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Base}/{action=Index}/{id?}");

app.Run();
