using BritInsurance.Application.Dto;
using BritInsurance.Application.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BritInsurance.Application.Tests.Validation
{
    public class CreateItemValidatorTests
    {
        [Fact]
        public async Task Validate_ValidItem_ReturnsSuccess()
        {
            // Arrange
            var validator = new CreateItemValidator();
            var item = new CreateItemDto
            {
                ProductId = 1,
                Quantity = 5
            };
            
            // Act
            var result = await validator.ValidateAsync(item);
            
            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task Validate_ProductIdIsRequired_NotValid()
        {
            // Arrange
            var validator = new CreateItemValidator();
            var item = new CreateItemDto
            {
                Quantity = 5
            };

            // Act
            var result = await validator.ValidateAsync(item);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(CreateItemDto.ProductId), result.Errors[0].PropertyName);
            Assert.Equal("Product ID is required.", result.Errors[0].ErrorMessage);
        }
    }
}
