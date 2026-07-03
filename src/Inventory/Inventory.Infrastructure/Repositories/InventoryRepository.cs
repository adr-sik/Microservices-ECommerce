using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly InventoryDbContext _context;

        public InventoryRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<List<InventoryItem>> GetAsync()
        {
            return await _context.Items.ToListAsync();
        }

        public async Task<InventoryItem?> GetAsync(Guid id)
        {
            return await _context.Items.FindAsync(id);
        }

        public async Task<InventoryItem> CreateAsync(string productId)
        {
            var item = _context.Items.Add(InventoryItem.Create(productId));
            await _context.SaveChangesAsync();
            return item.Entity;
        }

        public async Task UpdateAsync(InventoryItem item)
        {
            _context.Items.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null) return;
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<List<InventoryItem>> GetByProductIdsAsync(List<string> productIds, CancellationToken ct = default)
        {
            var result = await _context.Items
                .Where(i => productIds.Contains(i.ProductId))
                .ToListAsync(ct);
            return result;
        }

        public async Task UpdateManyAsync(List<InventoryItem> items, CancellationToken ct = default)
        {
            _context.Items.UpdateRange(items);
            await _context.SaveChangesAsync(ct);
        }
    }
}
