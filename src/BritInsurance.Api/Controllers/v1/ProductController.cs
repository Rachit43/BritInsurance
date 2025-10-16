using Asp.Versioning;
using BritInsurance.Application.Dto;
using BritInsurance.Application.Interface;
using BritInsurance.Infrastructure.Config;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BritInsurance.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/product")]
    [ApiController]
    [Authorize(Roles = ApplicationRoles.View)]
    public class ProductController(IProductService productService) : ControllerBase
    {
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            GetProductDto? product = await productService.GetByIdAsync(id);
            if (product == null)
            {
                return NoContent();
            }
            return Ok(product);
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetProductDto[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            GetProductDto[] product = await productService.GetAllAsync();
            if (product == null || product.Length == 0)
            {
                return NoContent();
            }
            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateProductDto request)
        {
            int id = await productService.AddAsync(request);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateProductDto[] request)
        {
            await productService.AddAsync(request);
            return new StatusCodeResult((int)HttpStatusCode.Created);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateRequest([FromServices] IValidator<UpdateProductDto> updateProductValidator, 
            [FromBody][CustomizeValidator(Skip = true)] UpdateProductDto request, 
            [FromRoute] int id, [FromQuery] bool ignoreItems = false)
        {
            request.IgnoreItems = ignoreItems;
            FluentValidation.Results.ValidationResult validationResult = await updateProductValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));
            }

            await productService.UpdateAsync(request, id, ignoreItems);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteRequest([FromRoute] int id)
        {
            await productService.DeleteAsync(id);

            return NoContent();
        }
    }
}