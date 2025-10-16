namespace BritInsurance.Domain.Interface
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(int id, bool enableChangeTracking);
        Task<TEntity[]> GetByIdsAsync(IEnumerable<int> id, bool enableChangeTracking);
        Task<TEntity[]> GetAllAsync();
        Task AddAsync(TEntity request);
        Task AddAsync(IEnumerable<TEntity> request);
        void Update(TEntity request);
        void Delete(TEntity request);
        void DeleteRange(IEnumerable<TEntity> request);
    }
}