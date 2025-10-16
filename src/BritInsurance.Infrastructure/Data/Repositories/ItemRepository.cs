using BritInsurance.Domain.Entities;
using BritInsurance.Domain.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BritInsurance.Infrastructure.Data.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly BritInsuranceDbContext _context;
        private readonly ILogger<ItemRepository> _logger;

        public ItemRepository(BritInsuranceDbContext context, ILogger<ItemRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddAsync(Item request)
        {
            await _context.Items.AddAsync(request);
        }

        public async Task AddAsync(IEnumerable<Item> request)
        {
            await _context.Items.AddRangeAsync(request);
        }

        public void Delete(Item entity)
        {
            _context.Items.Remove(entity);
        }

        public void DeleteRange(IEnumerable<Item> request)
        {
            _context.Items.RemoveRange(request);
        }

        public async Task<Item[]> GetAllAsync()
        {
            return await _context.Items
                .AsNoTracking()
                .ToArrayAsync();
        }

        public async Task<Item?> GetByIdAsync(int id, bool enableChangeTracking)
        {
            IQueryable<Item> dbSet = enableChangeTracking ? _context.Items : _context.Items.AsNoTracking();

            Item? entity = await dbSet.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                _logger.LogWarning("Item with id {Id} not found.", id);
            }

            return entity;
        }

        public async Task<Item[]> GetByIdsAsync(IEnumerable<int> id, bool enableChangeTracking)
        {
            IQueryable<Item> dbSet = enableChangeTracking ? _context.Items : _context.Items.AsNoTracking();

            Item[] entities = await dbSet
                .Where(x => id.Contains(x.Id))
                .ToArrayAsync();

            return entities;
        }

        public void Update(Item request)
        {
            _context.Items.Update(request);
        }
    }
}
