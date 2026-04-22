using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartLogisticsHub.Core.Patterns.Creational;
using SmartLogisticsHub.Infrastructure.Data;
using System.Linq;
using System.Threading.Tasks;
using SmartLogisticsHub.Core.Abstractions;

namespace SmartLogisticsHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly IItemRepository _itemRepository;

    public ReportController(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    [HttpGet("inventory-summary")]
    public async Task<IActionResult> GetInventoryReport()
    {
        var itemCount = await _itemRepository.CountAsync();
        var totalWeight = await _itemRepository.TotalWeightAsync();

        var director = new ReportDirector();
        var builder = new InventoryReportBuilder();
        director.Construct(builder, itemCount, totalWeight);
        var report = builder.GetResult();

        return Ok(new { report.Sections, report.Summary });
    }
}
