using Microsoft.AspNetCore.Mvc;
using SmartLogisticsHub.Core.Abstractions;
using SmartLogisticsHub.Core.Models;
using SmartLogisticsHub.Core.Patterns.Creational;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLogisticsHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly IItemRepository _itemRepository;

    public InventoryController(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Item>>> GetItems()
    {
        return Ok(await _itemRepository.GetAllAsync());
    }

    [HttpPost]
    public async Task<ActionResult<Item>> CreateItem(Item item)
    {
        await _itemRepository.AddAsync(item);
        await _itemRepository.SaveChangesAsync();
        return CreatedAtAction(nameof(GetItems), new { id = item.Id }, item);
    }

    [HttpPost("clone/{id}")]
    public async Task<ActionResult<Item>> CloneItem(Guid id, [FromQuery] string newSku)
    {
        var original = await _itemRepository.GetByIdAsync(id);
        if (original == null) return NotFound();

        var clone = original.Clone();
        clone.Id = Guid.NewGuid();
        clone.SKU = newSku;
        clone.Name += " (Clone)";

        await _itemRepository.AddAsync(clone);
        await _itemRepository.SaveChangesAsync();
        return Ok(clone);
    }

    [HttpGet("config")]
    public IActionResult GetWarehouseConfig()
    {
        return Ok(WarehouseConfig.Instance());
    }
}
