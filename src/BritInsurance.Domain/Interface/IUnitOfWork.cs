namespace BritInsurance.Domain.Interface
{
    public interface IUnitOfWork
    {
        public IItemRepository Items { get; }

        public IProductRepository Products { get; }

        public Task SaveChangesAsync();
    }
}
