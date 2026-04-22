using Microsoft.AspNetCore.Mvc;
using SmartLogisticsHub.Core.Abstractions;
using SmartLogisticsHub.Core.Models;
using SmartLogisticsHub.Core.Patterns.Behavioral;

namespace SmartLogisticsHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BehavioralController : ControllerBase
{
    private readonly IItemRepository _itemRepository;

    public BehavioralController(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    [HttpGet("strategy")]
    public IActionResult RunStrategy([FromQuery] double weight, [FromQuery] string type)
    {
        var context = new ShippingContext();
        if (type == "express") context.SetStrategy(new ExpressShippingStrategy());
        else context.SetStrategy(new StandardShippingStrategy());

        return Ok(new { Cost = context.ContextInterface(weight) });
    }

    [HttpPost("observer")]
    public IActionResult RunObserver([FromBody] Order order)
    {
        var dispatcher = new OrderDispatcher();
        dispatcher.RegisterObserver(new EmailObserver());
        dispatcher.RegisterObserver(new SmsObserver());

        return Ok(dispatcher.NotifyObservers(order));
    }
    
    [HttpPost("state")]
    public IActionResult RunState([FromBody] Order order)
    {
        var context = new StateContext(order);
        context.Request(); 
        return Ok(context.Order);
    }
    
    [HttpPost("template")]
    public IActionResult RunTemplate([FromBody] Order order, [FromQuery] bool isVip)
    {
        OrderTemplate template = isVip ? new VipOrderTemplate() : new RegularOrderTemplate();
        return Ok(template.ProcessDocument(order));
    }

    [HttpGet("iterator")]
    public async Task<IActionResult> RunIterator()
    {
        var items = await _itemRepository.GetAllAsync();
        IAggregate<Item> aggregate = new ItemAggregate(items);
        IIterator<Item> iterator = aggregate.CreateIterator();

        var result = new List<Item>();
        while (iterator.HasNext())
        {
            result.Add(iterator.Next());
        }
        return Ok(result);
    }
}