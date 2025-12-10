using ERP_System.Data;
using ERP_System.Models;
using ERP_System.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ERP_System.Services.Implementations
{
    public class StoreService : IStoreService
    {
        private readonly AppDbContext _context;

        public StoreService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Store>> GetAllAsync()
        {
            return await _context.Stores
                .AsNoTracking()
                .Include(s => s.Phones)
                .Include(s => s.Images)
                .ToListAsync();
        }

        public async Task<Store?> GetByIdAsync(int id)
        {
            return await _context.Stores
                .Include(s => s.Phones)
                .Include(s => s.Images)
                .Include(s => s.Inventories)
                    .ThenInclude(i => i.Item)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> IsNameExistsAsync(string name, int? excludeId = null)
        {
            if (excludeId.HasValue)
                return await _context.Stores.AnyAsync(s => s.Name == name && s.Id != excludeId.Value);
            return await _context.Stores.AnyAsync(s => s.Name == name);
        }

        public async Task AddAsync(Store store, List<string> phones, byte[]? imageData)
        {
            _context.Stores.Add(store);
   

            if (phones != null && phones.Any())
            {
                store.Phones = new List<StorePhone>(); // Initialize if needed or use context add
                foreach (var ph in phones.Where(p => !string.IsNullOrWhiteSpace(p)))
                {
                    // Adding to collection is cleaner
                    store.Phones.Add(new StorePhone { Phone = ph.Trim() });
                }
            }

            if (imageData != null)
            {
                store.Images = new List<StoreImage> { new StoreImage { ImageData = imageData } };
            }

        
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Store store, List<string> phones, byte[]? imageData)
        {
            var existingStore = await GetByIdAsync(store.Id);
            if (existingStore == null) return;

            existingStore.Name = store.Name;
            existingStore.Address = store.Address;
            existingStore.ManagerName = store.ManagerName;
            existingStore.Email = store.Email;
            existingStore.IsActive = store.IsActive;
            existingStore.OpeningHours = store.OpeningHours;
            existingStore.Notes = store.Notes;

            // Phones
            if (existingStore.Phones != null) _context.StorePhones.RemoveRange(existingStore.Phones);
            if (phones != null && phones.Any())
            {
                foreach (var ph in phones.Where(p => !string.IsNullOrWhiteSpace(p)))
                {
                    _context.StorePhones.Add(new StorePhone { StoreId = existingStore.Id, Phone = ph.Trim() });
                }
            }

            // Image
            if (imageData != null)
            {
                var existingImage = existingStore.Images?.FirstOrDefault();
                if (existingImage != null)
                {
                    existingImage.ImageData = imageData;
                    _context.StoreImages.Update(existingImage);
                }
                else
                {
                    _context.StoreImages.Add(new StoreImage { StoreId = existingStore.Id, ImageData = imageData });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var store = await GetByIdAsync(id);
            if (store == null) return;

            if (store.Phones != null && store.Phones.Any()) _context.StorePhones.RemoveRange(store.Phones);
            if (store.Images != null && store.Images.Any()) _context.StoreImages.RemoveRange(store.Images);

            _context.Stores.Remove(store);
            await _context.SaveChangesAsync();
        }
    }
}
