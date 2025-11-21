using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP_System.Models;
using ERP_System.Data;
using ERP_System.ViewModels;

namespace ERP_System.Controllers
{
    public class DefineItemController : Controller
    {
        private readonly AppDbContext _context;

        public DefineItemController(AppDbContext context)
        {
            _context = context;
        }

        // List all items
        public async Task<IActionResult> List()
        {
            var items = await _context.Items
                .Include(i => i.Company)
                .Include(i => i.Store)
                .Include(i => i.Unit)
                .Include(i => i.Codes)
                .Include(i => i.Images)
                .ToListAsync();
            return View(items);
        }

        // Create new item (GET)
        public IActionResult Index()
        {
            LoadViewBagData();
            return View();
        }

        // Create new item (POST)
        [HttpPost]
        public IActionResult Index(AddItemVm advm)
        {
            byte[] imageBytes = null;

            if (advm.Image != null)
            {
                using (var ms = new MemoryStream())
                {
                    advm.Image.CopyTo(ms);
                    imageBytes = ms.ToArray();
                }
            }

            var item = new Item
            {
                Name = advm.Name,
                CompanyMade = advm.CompanyMade,
                DefaultStore = advm.DefaultStore,
                BuyPrice = advm.BuyPrice,
                IsActiveBuy = advm.IsActiveBuy,
                IsActiveSale = advm.IsActiveSale,
                MinimumQuantity = advm.MinimumQuantity,
                MinQuantitySale = advm.MinQuantitySale,
                PreventDiscount = advm.PreventDiscount,
                PreventFraction = advm.PreventFraction,
                SalePrice = advm.SalePrice,
                UnitNumber = advm.UnitNumber
            };

            _context.Items.Add(item);

            if (advm.Codes != null)
            {
                foreach (var c in advm.Codes)
                {
                    var code = new ItemCode
                    {
                        Item = item,
                        ItemCodeValue = c
                    };
                    _context.ItemCodes.Add(code);
                }
            }

            if (advm.CategoryIds != null)
            {
                foreach (var c in advm.CategoryIds.Distinct())
                {
                    var cat = new ItemCategory
                    {
                        Item = item,
                        CategoryId = c,
                    };

                    _context.ItemCategories.Add(cat);
                }
            }

            if (imageBytes != null)
            {
                var img = new ItemImage
                {
                    Item = item,
                    ItemImageData = imageBytes
                };

                _context.ItemImages.Add(img);
            }

            _context.SaveChanges();
            return RedirectToAction("List");
        }

        // View item details
        public async Task<IActionResult> Details(int id)
        {
            var item = await _context.Items
                .Include(i => i.Company)
                .Include(i => i.Store)
                .Include(i => i.Unit)
                .Include(i => i.Codes)
                .Include(i => i.Images)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // Edit item (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.Items
                .Include(i => i.Codes)
                .Include(i => i.Images)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            var categoryIds = await _context.ItemCategories
                .Where(ic => ic.ItemId == id)
                .Select(ic => ic.CategoryId)
                .ToListAsync();

            var viewModel = new AddItemVm
            {
                Name = item.Name,
                IsActiveBuy = item.IsActiveBuy ?? false,
                IsActiveSale = item.IsActiveSale ?? false,
                CompanyMade = item.CompanyMade,
                DefaultStore = item.DefaultStore,
                UnitNumber = item.UnitNumber,
                MinimumQuantity = item.MinimumQuantity,
                MinQuantitySale = item.MinQuantitySale,
                PreventFraction = item.PreventFraction,
                PreventDiscount = item.PreventDiscount,
                BuyPrice = item.BuyPrice,
                SalePrice = item.SalePrice,
                Codes = item.Codes?.Select(c => c.ItemCodeValue).ToList() ?? new List<string>(),
                CategoryIds = categoryIds
            };

            LoadViewBagData();
            ViewBag.ItemId = id;
            return View(viewModel);
        }

        // Edit item (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(int id, AddItemVm advm)
        {
            var item = await _context.Items
                .Include(i => i.Codes)
                .Include(i => i.Images)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            // Update item properties
            item.Name = advm.Name;
            item.IsActiveBuy = advm.IsActiveBuy;
            item.IsActiveSale = advm.IsActiveSale;
            item.CompanyMade = advm.CompanyMade;
            item.DefaultStore = advm.DefaultStore;
            item.UnitNumber = advm.UnitNumber;
            item.MinimumQuantity = advm.MinimumQuantity;
            item.MinQuantitySale = advm.MinQuantitySale;
            item.PreventFraction = advm.PreventFraction;
            item.PreventDiscount = advm.PreventDiscount;
            item.BuyPrice = advm.BuyPrice;
            item.SalePrice = advm.SalePrice;

            // Update codes
            _context.ItemCodes.RemoveRange(item.Codes);
            if (advm.Codes != null)
            {
                foreach (var c in advm.Codes)
                {
                    var code = new ItemCode
                    {
                        ItemId = item.Id,
                        ItemCodeValue = c
                    };
                    _context.ItemCodes.Add(code);
                }
            }

            // Update categories
            var existingCategories = await _context.ItemCategories
                .Where(ic => ic.ItemId == id)
                .ToListAsync();
            _context.ItemCategories.RemoveRange(existingCategories);

            if (advm.CategoryIds != null)
            {
                foreach (var catId in advm.CategoryIds.Distinct())
                {
                    var cat = new ItemCategory
                    {
                        ItemId = item.Id,
                        CategoryId = catId
                    };
                    _context.ItemCategories.Add(cat);
                }
            }

            // Update image if provided
            if (advm.Image != null)
            {
                byte[] imageBytes;
                using (var ms = new MemoryStream())
                {
                    advm.Image.CopyTo(ms);
                    imageBytes = ms.ToArray();
                }

                var existingImage = item.Images?.FirstOrDefault();
                if (existingImage != null)
                {
                    existingImage.ItemImageData = imageBytes;
                }
                else
                {
                    var img = new ItemImage
                    {
                        ItemId = item.Id,
                        ItemImageData = imageBytes
                    };
                    _context.ItemImages.Add(img);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("List");
        }

        // Delete item
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Items
                .Include(i => i.Codes)
                .Include(i => i.Images)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return RedirectToAction("List");
        }

        // Helper method to load ViewBag data
        private void LoadViewBagData()
        {
            var companies = _context.Companies.Select(s => new CompnayVm
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();


            var stores = _context.Stores.Select(s => new StoreVm
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();

            var units = _context.Units.Select(s => new UnitVm
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();

            var cats = _context.Categories.Select(s => new CategoryVm
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();

            ViewBag.companies = companies;
            ViewBag.stores = stores;
            ViewBag.units = units;
            ViewBag.categories = cats;
        }
    }
}
