using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP_System.Models;
using ERP_System.Data;
using ERP_System.ViewModels;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ERP_System.Controllers
{
    [Authorize]
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
            return View(new AddItemVm());
        }

        // Create new item (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(AddItemVm advm)
        {
            // ???? ?? ??? ??????? ?????
            if (!ModelState.IsValid)
            {
                LoadViewBagData();
                return View(advm);
            }

            byte[]? imageBytes = null;
            if (advm.Image != null)
            {
                using var ms = new MemoryStream();
                await advm.Image.CopyToAsync(ms);
                imageBytes = ms.ToArray();
            }

            var item = new Item
            {
                Name = advm.Name?.Trim(),
                Description = advm.Description?.Trim(),
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

            try
            {
                _context.Items.Add(item);

                // ????? ?????? - ????? ????? ??????? ??? ???????
                if (advm.Codes != null)
                {
                    foreach (var codeValue in advm.Codes.Where(c => !string.IsNullOrWhiteSpace(c)).Select(c => c.Trim()))
                    {
                        var code = new ItemCode
                        {
                            Item = item,
                            ItemCodeValue = codeValue
                        };
                        _context.ItemCodes.Add(code);
                    }
                }

                // ?????????
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

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // ????? ???? ??????? ???????
                ModelState.AddModelError("", "????? ??? ?????. ???? ?? ??? ???????? ????? ??? ????.");
                LoadViewBagData();
                return View(advm);
            }

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

            if (item == null) return NotFound();

            return View(item);
        }

        // Edit item (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.Items
                .Include(i => i.Codes)
                .Include(i => i.Images)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (item == null) return NotFound();

            var categoryIds = await _context.ItemCategories
                .Where(ic => ic.ItemId == id)
                .Select(ic => ic.CategoryId)
                .ToListAsync();

            var viewModel = new AddItemVm
            {
                Name = item.Name,
                Description = item.Description,
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

            // ?????? ?????? ?????? ?? ??? View ????? ????? Base64 ?? ViewBag (???????)
            if (item.Images != null && item.Images.Any() && item.Images.First().ItemImageData != null)
            {
                ViewBag.ImageBase64 = Convert.ToBase64String(item.Images.First().ItemImageData);
            }

            LoadViewBagData();
            ViewBag.ItemId = id;
            return View(viewModel);
        }

        // Edit item (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AddItemVm advm)
        {
            if (!ModelState.IsValid)
            {
                LoadViewBagData();
                ViewBag.ItemId = id;
                return View(advm);
            }

            var item = await _context.Items
                .Include(i => i.Codes)
                .Include(i => i.Images)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (item == null) return NotFound();

            // Update item properties (?? Trim ????? ?????)
            item.Name = advm.Name?.Trim();
            item.Description = advm.Description?.Trim();
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

            // Update codes: ??? ?????? ?? ????? ?????? (??? ?????)
            _context.ItemCodes.RemoveRange(item.Codes ?? Enumerable.Empty<ItemCode>());
            if (advm.Codes != null)
            {
                foreach (var codeValue in advm.Codes.Where(c => !string.IsNullOrWhiteSpace(c)).Select(c => c.Trim()))
                {
                    var code = new ItemCode
                    {
                        ItemId = item.Id,
                        ItemCodeValue = codeValue
                    };
                    _context.ItemCodes.Add(code);
                }
            }

            // Update categories: ??? ?????? ?? ????? ??????
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
                    await advm.Image.CopyToAsync(ms);
                    imageBytes = ms.ToArray();
                }

                var existingImage = item.Images?.FirstOrDefault();
                if (existingImage != null)
                {
                    existingImage.ItemImageData = imageBytes;
                }
                else
                {
                    _context.ItemImages.Add(new ItemImage
                    {
                        ItemId = item.Id,
                        ItemImageData = imageBytes
                    });
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "????? ??? ?????????. ???? ?? ??? ???????? ????? ??? ????.");
                LoadViewBagData();
                ViewBag.ItemId = id;
                return View(advm);
            }

            return RedirectToAction("List");
        }

        // Delete item
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Items
                .Include(i => i.Codes)
                .Include(i => i.Images)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (item == null) return NotFound();

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
