using BritInsurance.Api.Controllers.v1;
using BritInsurance.Application.Dto;
using BritInsurance.Application.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BritInsurance.Api.Tests.Controllers.v1
{
    public class ItemControllerTests
    {
        private readonly Mock<IItemService> _itemServiceMock;

        public ItemControllerTests()
        {
            _itemServiceMock = new();
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenItemExists()
        {
            // Arrange
            var itemId = 1;
            var itemDto = new GetItemDto { Id = itemId, ProductId = 1, Quantity = 5 };
            _itemServiceMock.Setup(x => x.GetByIdAsync(itemId)).ReturnsAsync(itemDto);
            var controller = new ItemController(_itemServiceMock.Object);

            // Act
            var result = await controller.GetById(itemId);
            
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(itemDto, okResult.Value);

            // Verify
            _itemServiceMock.Verify(x => x.GetByIdAsync(itemId), Times.Once);
            _itemServiceMock.VerifyAll();
        }

        [Fact]
        public async Task GetById_ReturnsNoContent_WhenItemDoesNotExists()
        {
            // Arrange
            var itemId = 1;
            var itemDto = new GetItemDto { Id = itemId, ProductId = 1, Quantity = 5 };
            _itemServiceMock.Setup(x => x.GetByIdAsync(itemId)).ReturnsAsync((GetItemDto?)null);
            var controller = new ItemController(_itemServiceMock.Object);

            // Act
            var result = await controller.GetById(itemId);

            // Assert
            var okResult = Assert.IsType<NoContentResult>(result);

            // Verify
            _itemServiceMock.Verify(x => x.GetByIdAsync(itemId), Times.Once);
            _itemServiceMock.VerifyAll();
        }
    }
}
