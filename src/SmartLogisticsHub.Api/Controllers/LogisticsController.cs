using Microsoft.AspNetCore.Mvc;
using SmartLogisticsHub.Core.Abstractions;
using SmartLogisticsHub.Core.Models;
using SmartLogisticsHub.Core.Patterns.Structural;
using SmartLogisticsHub.Core.Services;
using System;
using System.Threading.Tasks;

namespace SmartLogisticsHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LogisticsController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICargoRepository _cargoRepository;
    private readonly IShippingProvider _shippingProvider;

    public LogisticsController(
        IOrderRepository orderRepository, 
        ICargoRepository cargoRepository,
        IShippingProvider shippingProvider)
    {
        _orderRepository = orderRepository;
        _cargoRepository = cargoRepository;
        _shippingProvider = shippingProvider;
    }

    [HttpGet("orders/{id}")]
    public async Task<IActionResult> GetOrder(Guid id) => Ok(await _orderRepository.GetByIdAsync(id));

    [HttpPost("orders/dispatch/{id}")]
    public async Task<IActionResult> DispatchOrder(Guid id, [FromQuery] string address, [FromQuery] bool insured)
    {
        var facade = new OrderDispatchFacade(_orderRepository, _shippingProvider);
        return Ok(await facade.DispatchOrder(id, address, insured));
    }

    [HttpGet("shipment/status/{trackingId}")]
    public IActionResult GetStatus(string trackingId)
    {
        var status = _shippingProvider.GetTrackingStatus(trackingId);
        return Ok(new { TrackingId = trackingId, Status = status });
    }

    [HttpPost("cargo")]
    public async Task<IActionResult> CreateCargo(CargoEntity entity)
    {
        await _cargoRepository.AddAsync(entity);
        await _cargoRepository.SaveChangesAsync();
        return Ok(entity);
    }

    [HttpGet("cargo/{rootId}")]
    public async Task<IActionResult> GetCargoHierarchy(Guid rootId)
    {
        var entity = await _cargoRepository.GetRootWithChildrenAsync(rootId);
        if (entity == null) return NotFound();

        var component = CargoBundle.FromEntity(entity);
        return Ok(new { Name = component.Name, TotalWeight = component.GetTotalWeight() });
    }
    
    [HttpGet("orders")]
    public async Task<IActionResult> GetOrders() => Ok(await _orderRepository.GetAllAsync());

    [HttpPost("orders")]
    public async Task<IActionResult> CreateOrder([FromBody] Order order)
    {
        await _orderRepository.AddAsync(order);
        await _orderRepository.SaveChangesAsync();
        return Ok(order);
    }

    [HttpPut("orders")]
    public async Task<IActionResult> UpdateOrder([FromBody] Order order)
    {
        await _orderRepository.UpdateAsync(order);
        await _orderRepository.SaveChangesAsync();
        return Ok(order);
    }
}
