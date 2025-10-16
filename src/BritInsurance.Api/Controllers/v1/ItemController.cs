using Asp.Versioning;
using BritInsurance.Application.Dto;
using BritInsurance.Application.Interface;
using BritInsurance.Infrastructure.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BritInsurance.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/item")]
    [ApiController]
    [Authorize(Roles = ApplicationRoles.View)]
    public class ItemController(IItemService itemService) : ControllerBase
    {
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            GetItemDto? Item = await itemService.GetByIdAsync(id);
            if (Item == null)
            {
                return NoContent();
            }
            return Ok(Item);
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetItemDto[]), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll()
        {
            GetItemDto[] Item = await itemService.GetAllAsync();
            if (Item == null || Item.Length == 0)
            {
                return NoContent();
            }
            return Ok(Item);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateItemDto request)
        {
            int id = await itemService.AddAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = id }, null);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateItemDto[] request)
        {
            await itemService.AddAsync(request);
            return new StatusCodeResult((int)HttpStatusCode.Created);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateRequest([FromBody] UpdateItemDto request, [FromRoute] int id)
        {
            await itemService.UpdateAsync(request, id);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteRequest([FromRoute] int id)
        {
            await itemService.DeleteAsync(id);

            return NoContent();
        }
    }
}