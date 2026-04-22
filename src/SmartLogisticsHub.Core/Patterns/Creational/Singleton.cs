namespace SmartLogisticsHub.Core.Patterns.Creational;

public class WarehouseConfig
{
    private static WarehouseConfig? _instance;
    public string HubCode { get; set; } = "SH-CENTRAL-01";
    public string OperatingCurrency { get; set; } = "USD";
    public double TaxRate { get; set; } = 0.15;

    protected WarehouseConfig() { }

    public static WarehouseConfig Instance()
    {
        if (_instance == null)
        {
            _instance = new WarehouseConfig();
        }
        return _instance;
    }
}
