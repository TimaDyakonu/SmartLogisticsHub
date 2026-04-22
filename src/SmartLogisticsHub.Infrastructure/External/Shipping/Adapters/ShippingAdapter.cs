using SmartLogisticsHub.Core.Abstractions;

namespace SmartLogisticsHub.Infrastructure.External.Shipping.Adapters;

public class LegacyShippingSystem
{
    public void RegisterPacket(string dest, double w, bool isPriority) { }
    public string FetchStatus(string legacyId) => $"Legacy System: Shipment {legacyId} is IN_TRANSIT";
}

public class LegacyShippingAdapter : IShippingProvider
{
    private readonly LegacyShippingSystem _legacySystem;

    public LegacyShippingAdapter(LegacyShippingSystem legacySystem)
    {
        _legacySystem = legacySystem;
    }

    public string Ship(string address, double weight)
    {
        _legacySystem.RegisterPacket(address, weight, weight < 5.0);
        return $"LEGACY_SHIP_{Guid.NewGuid().ToString()[..8]}";
    }

    public string GetTrackingStatus(string shipmentId)
    {
        return _legacySystem.FetchStatus(shipmentId);
    }
}
