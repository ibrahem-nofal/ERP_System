using Microsoft.AspNetCore.Mvc;
using ERP_System.Data;
using ERP_System.Models;
using ERP_System.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ERP_System.Controllers
{
    public class SettingsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SettingsController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        // ==========================================
        // Company Profile
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> CompanyProfile()
        {
            var company = await _context.Companies.FirstOrDefaultAsync();
            var logoSetting = await _context.Settings.FirstOrDefaultAsync(s => s.Key == "CompanyLogo");
            var model = new CompanyProfileViewModel();

            if (company != null)
            {
                model.Id = company.Id;
                model.Code = company.Code;
                model.Name = company.Name;
                model.Address = company.Address;
                model.TaxNumber = company.TaxNumber;
                model.ContactNumber = company.PhoneNumber;
            }

            if (logoSetting != null)
            {
                model.LogoPath = logoSetting.Value;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompanyProfile(CompanyProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var company = await _context.Companies.FirstOrDefaultAsync();
                if (company == null)
                {
                    company = new Company();
                    _context.Companies.Add(company);
                }

                company.Code = model.Code;
                company.Name = model.Name;
                company.Address = model.Address;
                company.TaxNumber = model.TaxNumber;
                company.PhoneNumber = model.ContactNumber;

                // Handle Logo Upload
                if (model.LogoFile != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.LogoFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.LogoFile.CopyToAsync(fileStream);
                    }

                    await SaveSettingAsync("CompanyLogo", uniqueFileName, "Company");
                }

                await _context.SaveChangesAsync();
                TempData["Success"] = "تم تحديث ملف الشركة بنجاح.";
                return RedirectToAction(nameof(CompanyProfile));
            }
            return View(model);
        }



        // ==========================================
        // Financial Settings
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> Financial()
        {
            var settings = await _context.Settings.Where(s => s.Group == "Financial").ToListAsync();

            var model = new FinancialSettingsViewModel
            {
                Currency = settings.FirstOrDefault(s => s.Key == "Currency")?.Value ?? "USD",
                TaxPercentage = decimal.TryParse(settings.FirstOrDefault(s => s.Key == "TaxPercentage")?.Value, out var tax) ? tax : 15,
                FiscalYearStart = DateTime.TryParse(settings.FirstOrDefault(s => s.Key == "FiscalYearStart")?.Value, out var date) ? date : DateTime.Now,
                AutoNumbering = bool.TryParse(settings.FirstOrDefault(s => s.Key == "AutoNumbering")?.Value, out var auto) ? auto : true
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Financial(FinancialSettingsViewModel model)
        {
            if (ModelState.IsValid)
            {
                await SaveSettingAsync("Currency", model.Currency, "Financial");
                await SaveSettingAsync("TaxPercentage", model.TaxPercentage.ToString(), "Financial");
                await SaveSettingAsync("FiscalYearStart", model.FiscalYearStart.ToString(), "Financial");
                await SaveSettingAsync("AutoNumbering", model.AutoNumbering.ToString(), "Financial");

                await _context.SaveChangesAsync();

                TempData["Success"] = "تم تحديث الإعدادات المالية بنجاح.";
                return RedirectToAction(nameof(Financial));
            }
            return View(model);
        }

        // ==========================================
        // Inventory Settings
        // ==========================================
        public async Task<IActionResult> Inventory()
        {
            var settings = await _context.Settings.Where(s => s.Group == "Inventory").ToListAsync();
            var model = new InventorySettingsViewModel
            {
                Units = await _context.Units.ToListAsync(),
                Categories = await _context.Categories.ToListAsync(),
                StockValuationMethod = settings.FirstOrDefault(s => s.Key == "StockValuationMethod")?.Value ?? "FIFO"
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Inventory(InventorySettingsViewModel model)
        {
            // Note: Units and Categories are managed via their own controllers/actions usually, 
            // but if we need to reload them for the view in case of error:
            if (ModelState.IsValid)
            {
                await SaveSettingAsync("StockValuationMethod", model.StockValuationMethod, "Inventory");
                await _context.SaveChangesAsync();

                TempData["Success"] = "تم تحديث إعدادات المخزون بنجاح.";
                return RedirectToAction(nameof(Inventory));
            }

            // Reload lists if returning view
            model.Units = await _context.Units.ToListAsync();
            model.Categories = await _context.Categories.ToListAsync();
            return View(model);
        }



        private async Task SaveSettingAsync(string key, string value, string group)
        {
            var setting = await _context.Settings.FirstOrDefaultAsync(s => s.Key == key);
            if (setting == null)
            {
                setting = new Setting { Key = key, Value = value ?? "", Group = group };
                _context.Settings.Add(setting);
            }
            else
            {
                setting.Value = value ?? "";
                setting.Group = group;
                _context.Settings.Update(setting);
            }
        }
    }
}
