using BritInsurance.Application.Dto;

namespace BritInsurance.Application.Interface
{
    public interface IItemService
    {
        Task<int> AddAsync(CreateItemDto request);
        Task AddAsync(IEnumerable<CreateItemDto> request);
        Task DeleteAsync(int id);
        Task<GetItemDto[]> GetAllAsync();
        Task<GetItemDto?> GetByIdAsync(int id);
        Task UpdateAsync(UpdateItemDto request, int id);
    }
}
