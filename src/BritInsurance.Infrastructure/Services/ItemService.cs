using AutoMapper;
using BritInsurance.Application.Dto;
using BritInsurance.Application.Interface;
using BritInsurance.Domain.Entities;
using BritInsurance.Domain.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BritInsurance.Infrastructure.Services
{
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;
        private readonly ILogger<ItemService> _logger;

        public ItemService(IUnitOfWork unitOfWork, IMapper mapper, IUserContext userContext, ILogger<ItemService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContext = userContext;
            _logger = logger;
        }

        public async Task<int> AddAsync(CreateItemDto request)
        {
            Item Item = _mapper.Map<Item>(request);

            Product? product = await _unitOfWork.Products.GetByIdAsync(request.ProductId, true) ?? throw new InvalidOperationException("Product does not exist");

            await _unitOfWork.Items.AddAsync(Item);
            await _unitOfWork.SaveChangesAsync();

            return Item.Id;
        }

        public async Task AddAsync(IEnumerable<CreateItemDto> request)
        {
            Item[] Items = _mapper.Map<Item[]>(request);

            DateTime currentDate = DateTime.UtcNow;

            int[] distinctProductIds = Items.Select(x => x.ProductId).Distinct().ToArray();

            Product[] products = await _unitOfWork.Products.GetByIdsAsync(distinctProductIds, true);

            if (products.Length != distinctProductIds.Length)
            {
                throw new InvalidOperationException("One or more products do not exist");
            }

            await _unitOfWork.Items.AddAsync(Items);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Item? item = await _unitOfWork.Items.GetByIdAsync(id, true);

            if (item == null)
            {
                _logger.LogWarning("Item with id {Id} not found.", id);
                return;
            }

            _unitOfWork.Items.Delete(item);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<GetItemDto[]> GetAllAsync()
        {
            Item[] Items = await _unitOfWork.Items.GetAllAsync();

            GetItemDto[] ItemDtos = _mapper.Map<GetItemDto[]>(Items);

            return ItemDtos;
        }

        public async Task<GetItemDto?> GetByIdAsync(int id)
        {
            Item? entity = await _unitOfWork.Items.GetByIdAsync(id, false);
            if (entity == null)
            {
                _logger.LogWarning("Item with id {Id} not found.", id);
                return null;
            }

            GetItemDto? ItemDto = _mapper.Map<GetItemDto>(entity);
            return ItemDto;
        }

        public async Task UpdateAsync(UpdateItemDto request, int id)
        {
            Item? existingItem = await _unitOfWork.Items.GetByIdAsync(id, true);

            if (existingItem == null)
            {
                _logger.LogWarning("Item with id {Id} not found for update.", id);
                return;
            }

            existingItem.ProductId = request.ProductId;
            existingItem.Quantity = request.Quantity;

            _unitOfWork.Items.Update(existingItem);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}