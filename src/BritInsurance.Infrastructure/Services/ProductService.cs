using AutoMapper;
using BritInsurance.Application.Dto;
using BritInsurance.Application.Interface;
using BritInsurance.Domain.Entities;
using BritInsurance.Domain.Interface;
using Microsoft.Extensions.Logging;

namespace BritInsurance.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IUserContext userContext, ILogger<ProductService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContext = userContext;
            _logger = logger;
        }

        public async Task<int> AddAsync(CreateProductDto request)
        {
            Product product = _mapper.Map<Product>(request);

            product.CreatedBy = _userContext.UserName;
            product.CreatedOn = DateTime.UtcNow;

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return product.Id;
        }

        public async Task AddAsync(IEnumerable<CreateProductDto> request)
        {
            Product[] products = _mapper.Map<Product[]>(request);

            var currentDate = DateTime.UtcNow;
            foreach (Product product in products)
            {
                product.CreatedBy = _userContext.UserName;
                product.CreatedOn = currentDate;
            }

            await _unitOfWork.Products.AddAsync(products);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Product? entity = await _unitOfWork.Products.GetByIdAsync(id, true);

            if (entity == null)
            {
                _logger.LogWarning("Product with id {Id} not found.", id);
                return;
            }

            _unitOfWork.Items.DeleteRange(entity.Items);

            _unitOfWork.Products.Delete(entity);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<GetProductDto[]> GetAllAsync()
        {
            Product[] products = await _unitOfWork.Products.GetAllAsync();

            GetProductDto[] productDtos = _mapper.Map<GetProductDto[]>(products);

            return productDtos;
        }

        public async Task<GetProductDto?> GetByIdAsync(int id)
        {
            Product? entity = await _unitOfWork.Products.GetByIdAsync(id, false);
            
            if (entity == null)
            {
                _logger.LogWarning("Product with id {Id} not found.", id);
                return null;
            }

            GetProductDto? productDto = _mapper.Map<GetProductDto>(entity);
            return productDto;
        }

        public async Task UpdateAsync(UpdateProductDto request, int id, bool ignoreItems)
        {
            Product? existingProduct = await _unitOfWork.Products.GetByIdAsync(id, true);
            if (existingProduct == null)
            {
                _logger.LogWarning("Product does not exist");

                return;
            }

            existingProduct.ProductName = request.ProductName;
            existingProduct.ModifiedBy = _userContext.UserName;
            existingProduct.ModifiedOn = DateTime.UtcNow;

            if (!ignoreItems)
            {
                UpdateChildItems(request, id, existingProduct);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        private void UpdateChildItems(UpdateProductDto request, int id, Product existingProduct)
        {
            foreach (var updatedItem in request.Items)
            {
                Item? existingItem = existingProduct.Items
                    .FirstOrDefault(i => i.Id == updatedItem.Id);

                if (existingItem != null && updatedItem.Id != 0)
                {
                    existingItem.Quantity = updatedItem.Quantity;
                }
                else
                {
                    existingProduct.Items.Add(new Item
                    {
                        Quantity = updatedItem.Quantity,
                        ProductId = id
                    });
                }
            }

            foreach (Item? existingItem in existingProduct.Items.ToList())
            {
                if (existingItem.Id != 0 && !request.Items.Any(i => i.Id == existingItem.Id))
                {
                    _unitOfWork.Items.Delete(existingItem);
                }
            }
        }
    }
}
