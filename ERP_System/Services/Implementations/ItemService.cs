using ERP_System.Data;
using ERP_System.Models;
using ERP_System.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ERP_System.Services.Implementations
{
    public class ItemService : IItemService
    {
        private readonly AppDbContext _context;

        public ItemService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Item>> GetAllAsync()
        {
            return await _context.Items
                .Include(i => i.Company)
                .Include(i => i.Store)
                .Include(i => i.Unit)
                .Include(i => i.Codes)
                .Include(i => i.Images)
                .ToListAsync();
        }

        public async Task<Item?> GetByIdAsync(int id)
        {
            return await _context.Items
                .Include(i => i.Company)
                .Include(i => i.Store)
                .Include(i => i.Unit)
                .Include(i => i.Codes)
                .Include(i => i.Images)
                .FirstOrDefaultAsync(i => i.Id == id); // Including Company/Store/Unit for Details view usually
        }

        public async Task<List<int>> GetCategoryIdsAsync(int itemId)
        {
            return await _context.Set<ItemCategory>()
               .Where(ic => ic.ItemId == itemId)
               .Select(ic => ic.CategoryId)
               .ToListAsync();
        }

        public async Task AddAsync(Item item, List<string> codes, List<int> categoryIds, byte[]? imageData)
        {
            _context.Items.Add(item);

            // Add Codes
            if (codes != null)
            {
                item.Codes = new List<ItemCode>();
                foreach (var c in codes.Where(x => !string.IsNullOrWhiteSpace(x)))
                {
                    item.Codes.Add(new ItemCode { ItemCodeValue = c.Trim() });
                }
            }

            
            await _context.SaveChangesAsync();
           

            if (categoryIds != null && categoryIds.Any())
            {
                foreach (var catId in categoryIds.Distinct())
                {
                    _context.Set<ItemCategory>().Add(new ItemCategory
                    {
                        ItemId = item.Id,
                        CategoryId = catId
                    });
                }
            }

            if (imageData != null)
            {
                _context.Set<ItemImage>().Add(new ItemImage
                {
                    ItemId = item.Id,
                    ItemImageData = imageData
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Item item, List<string> codes, List<int> categoryIds, byte[]? imageData)
        {
            var existingItem = await _context.Items
                .Include(i => i.Codes)
                .Include(i => i.Images)
                .FirstOrDefaultAsync(i => i.Id == item.Id);

            if (existingItem == null) return;

            // Update scalar props
            existingItem.Name = item.Name;
            existingItem.Description = item.Description;
            existingItem.CompanyMade = item.CompanyMade;
            existingItem.DefaultStore = item.DefaultStore;
            existingItem.UnitNumber = item.UnitNumber;
            existingItem.MinimumQuantity = item.MinimumQuantity;
            existingItem.MinQuantitySale = item.MinQuantitySale;
            existingItem.PreventDiscount = item.PreventDiscount;
            existingItem.PreventFraction = item.PreventFraction;
            existingItem.BuyPrice = item.BuyPrice;
            existingItem.SalePrice = item.SalePrice;
            existingItem.IsActiveBuy = item.IsActiveBuy;
            existingItem.IsActiveSale = item.IsActiveSale;

            // Codes
            _context.ItemCodes.RemoveRange(existingItem.Codes ?? Enumerable.Empty<ItemCode>());
            if (codes != null)
            {
                foreach (var c in codes.Where(x => !string.IsNullOrWhiteSpace(x)))
                {
                    _context.ItemCodes.Add(new ItemCode { ItemId = existingItem.Id, ItemCodeValue = c.Trim() });
                }
            }

            // Categories
            var existingCats = await _context.Set<ItemCategory>().Where(ic => ic.ItemId == item.Id).ToListAsync();
            _context.Set<ItemCategory>().RemoveRange(existingCats);
            if (categoryIds != null)
            {
                foreach (var cid in categoryIds.Distinct())
                {
                    _context.Set<ItemCategory>().Add(new ItemCategory { ItemId = item.Id, CategoryId = cid });
                }
            }

            // Image
            if (imageData != null)
            {
                var existingImage = existingItem.Images?.FirstOrDefault();
                if (existingImage != null)
                {
                    existingImage.ItemImageData = imageData;
                }
                else
                {
                    _context.Set<ItemImage>().Add(new ItemImage { ItemId = existingItem.Id, ItemImageData = imageData });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.Items
                .Include(i => i.Codes)
                .Include(i => i.Images)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (item == null) return;

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
