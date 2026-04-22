using SmartLogisticsHub.Core.Abstractions;

namespace SmartLogisticsHub.Core.Patterns.Structural;

public abstract class ShippingDecorator : IShippingProvider
{
    protected readonly IShippingProvider _provider;

    protected ShippingDecorator(IShippingProvider provider)
    {
        _provider = provider;
    }

    public virtual string Ship(string address, double weight) => _provider.Ship(address, weight);
    public virtual string GetTrackingStatus(string shipmentId) => _provider.GetTrackingStatus(shipmentId);
}

public class InsuranceDecorator : ShippingDecorator
{
    private readonly double _insuranceFee;

    public InsuranceDecorator(IShippingProvider provider, double fee) : base(provider)
    {
        _insuranceFee = fee;
    }

    public override string Ship(string address, double weight)
    {
        var result = base.Ship(address, weight);
        return $"{result} (Insured by Decorator: +{_insuranceFee:C})";
    }

    public override string GetTrackingStatus(string shipmentId)
    {
        return $"{base.GetTrackingStatus(shipmentId)} [Insurance Validated]";
    }
}
