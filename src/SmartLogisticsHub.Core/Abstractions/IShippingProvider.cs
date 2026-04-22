namespace SmartLogisticsHub.Core.Abstractions;

public interface IShippingProvider
{
    string Ship(string address, double weight);
    string GetTrackingStatus(string shipmentId);
}
