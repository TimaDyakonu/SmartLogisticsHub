using SmartLogisticsHub.Core.Abstractions;
using SmartLogisticsHub.Core.Patterns.Creational;
using SmartLogisticsHub.Core.Patterns.Structural;
using System;
using System.Threading.Tasks;

namespace SmartLogisticsHub.Core.Services;

public class OrderDispatchFacade
{
    private readonly IOrderRepository _orderRepository;
    private readonly IShippingProvider _shippingProvider;

    public OrderDispatchFacade(IOrderRepository orderRepository, IShippingProvider shippingProvider)
    {
        _orderRepository = orderRepository;
        _shippingProvider = shippingProvider;
    }

    public async Task<string> DispatchOrder(Guid orderId, string address, bool useInsurance)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null) return "Order not found";

        order.Status = "Dispatched";
        await _orderRepository.UpdateAsync(order);
        
        var docCreator = new WaybillCreator();
        var printResult = docCreator.CreateAndPrint();
        
        IShippingProvider finalProvider = useInsurance 
            ? new InsuranceDecorator(_shippingProvider, 25.0) 
            : _shippingProvider;

        var shipResult = finalProvider.Ship(address, 10.0);
        await _orderRepository.SaveChangesAsync();

        return $"Order {orderId} dispatched.\nDocs: {printResult}\nShipping ID: {shipResult}";
    }
}
