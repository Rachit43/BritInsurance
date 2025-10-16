using BritInsurance.Application.Dto;

namespace BritInsurance.Application.Interface
{
    public interface IProductService
    {
        Task<int> AddAsync(CreateProductDto request);

        Task AddAsync(IEnumerable<CreateProductDto> request);

        Task DeleteAsync(int id);

        Task<GetProductDto[]> GetAllAsync();

        Task<GetProductDto?> GetByIdAsync(int id);

        Task UpdateAsync(UpdateProductDto request, int id, bool ignoreItems);
    }
}
