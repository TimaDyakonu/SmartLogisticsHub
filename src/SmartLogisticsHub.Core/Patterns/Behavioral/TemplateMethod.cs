using SmartLogisticsHub.Core.Models;
using SmartLogisticsHub.Core.Patterns.Creational;

namespace SmartLogisticsHub.Core.Patterns.Behavioral;

// AbstractClass
public abstract class OrderTemplate
{
    // TemplateMethod
    public Order ProcessDocument(Order order)
    {
        Validate(order);
        ApplyDiscount(order);
        AssignCarrier(order);
        return order;
    }

    // PrimitiveOperations
    protected virtual void Validate(Order order) { }
    protected abstract void ApplyDiscount(Order order);
    protected abstract void AssignCarrier(Order order);
}

// ConcreteClassA
public class VipOrderTemplate : OrderTemplate
{
    protected override void ApplyDiscount(Order order) => order.CustomerName += " [VIP]";
    
    protected override void AssignCarrier(Order order)
    {
        ShippingFamilyFactory factory = new ExpressShippingFactory();
        var label = factory.CreateLabel().GetFormat();
        var manifest = factory.CreateManifest().GetDetails();
        
        order.Status = $"Express Carrier [{label} | {manifest}]";
    }
}

// ConcreteClassB
public class RegularOrderTemplate : OrderTemplate
{
    protected override void ApplyDiscount(Order order) { }
    
    protected override void AssignCarrier(Order order)
    {
        ShippingFamilyFactory factory = new StandardShippingFactory();
        var label = factory.CreateLabel().GetFormat();
        var manifest = factory.CreateManifest().GetDetails();

        order.Status = $"Standard Carrier[{label} | {manifest}]";
    }
}