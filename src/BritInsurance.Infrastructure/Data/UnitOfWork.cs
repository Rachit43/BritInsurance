using BritInsurance.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BritInsurance.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BritInsuranceDbContext _context;

        public UnitOfWork(BritInsuranceDbContext context, IItemRepository itemRepository, IProductRepository productRepository)
        {
            _context = context;
            Items = itemRepository;
            Products = productRepository;
        }

        public IItemRepository Items { get; }

        public IProductRepository Products { get; }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
