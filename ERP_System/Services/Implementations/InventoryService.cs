using ERP_System.Data;
using ERP_System.Models;
using ERP_System.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ERP_System.Services.Implementations
{
    public class InventoryService : IInventoryService
    {
        private readonly AppDbContext _context;

        public InventoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddStockAsync(int itemId, int storeId, int quantity, string notes, int? refId, string transactionType)
        {
            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.ItemId == itemId && i.StoreId == storeId);

            if (inventory == null)
            {
                inventory = new Inventory
                {
                    ItemId = itemId,
                    StoreId = storeId,
                    CurrentQuantity = 0
                };
                _context.Inventories.Add(inventory);
            }

            inventory.CurrentQuantity += quantity;
            inventory.LastUpdated = DateTime.Now;

            var transaction = new InventoryTransaction
            {
                ItemId = itemId,
                StoreId = storeId,
                TransactionDate = DateTime.Now,
                Quantity = quantity,
                TransactionType = transactionType,
                Remarks = notes,
                ReferenceId = refId
            };

            _context.InventoryTransactions.Add(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveStockAsync(int itemId, int storeId, int quantity, string notes, int? refId, string transactionType)
        {
            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.ItemId == itemId && i.StoreId == storeId);

            if (inventory == null)
            {
                inventory = new Inventory
                {
                    ItemId = itemId,
                    StoreId = storeId,
                    CurrentQuantity = 0
                };
                _context.Inventories.Add(inventory);
            }

            inventory.CurrentQuantity -= quantity;
            inventory.LastUpdated = DateTime.Now;

            var transaction = new InventoryTransaction
            {
                ItemId = itemId,
                StoreId = storeId,
                TransactionDate = DateTime.Now,
                Quantity = quantity,
                TransactionType = transactionType,
                Remarks = notes,
                ReferenceId = refId
            };

            _context.InventoryTransactions.Add(transaction);
            await _context.SaveChangesAsync();
        }
    }
}
