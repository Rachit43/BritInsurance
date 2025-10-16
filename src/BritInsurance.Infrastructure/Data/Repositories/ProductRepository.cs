using BritInsurance.Domain.Entities;
using BritInsurance.Domain.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BritInsurance.Infrastructure.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly BritInsuranceDbContext _context;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(BritInsuranceDbContext context, ILogger<ProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddAsync(Product request)
        {
            await _context.Products.AddAsync(request);
        }

        public async Task AddAsync(IEnumerable<Product> request)
        {
            await _context.Products.AddRangeAsync(request);
        }

        public void Delete(Product entity)
        {
            _context.Products.Remove(entity);
        }

        public async Task<Product[]> GetAllAsync()
        {
            return await _context.Products
                .Include(x => x.Items)
                .AsNoTracking()
                .ToArrayAsync();
        }

        public async Task<Product?> GetByIdAsync(int id, bool enableChangeTracking)
        {
            IQueryable<Product> dbSet = enableChangeTracking ? _context.Products : _context.Products.AsNoTracking();

            Product? entity = await dbSet
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == id);
            
            if (entity == null)
            {
                _logger.LogWarning("Product with id {Id} not found.", id);
            }

            return entity;
        }

        public void Update(Product request)
        {
            _context.Products.Update(request);
        }

        public async Task<Product[]> GetByIdsAsync(IEnumerable<int> id, bool enableChangeTracking)
        {
            IQueryable<Product> dbSet = enableChangeTracking ? _context.Products : _context.Products.AsNoTracking();

            Product[] entities = await dbSet
                .Where(x => id.Contains(x.Id))
                .ToArrayAsync();

            return entities;
        }

        public void DeleteRange(IEnumerable<Product> request)
        {
            _context.Products.RemoveRange(request);
        }
    }
}
