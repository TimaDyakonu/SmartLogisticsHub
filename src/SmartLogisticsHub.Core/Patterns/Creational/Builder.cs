using System.Collections.Generic;

namespace SmartLogisticsHub.Core.Patterns.Creational;

public class WarehouseReport
{
    public List<string> Sections { get; } = new List<string>();
    public string Summary { get; set; }

    public void AddSection(string section) => Sections.Add(section);
}

public abstract class ReportBuilder
{
    public abstract void BuildHeader();
    public abstract void BuildStatistics(int itemCount, double totalWeight);
    public abstract void BuildFooter();
    public abstract WarehouseReport GetResult();
}

public class InventoryReportBuilder : ReportBuilder
{
    private WarehouseReport _report = new WarehouseReport();

    public override void BuildHeader() => _report.AddSection("SmartLogisticsHub - Inventory Snapshot");
    public override void BuildStatistics(int itemCount, double totalWeight) => _report.AddSection($"Items Count: {itemCount}, Total Weight: {totalWeight:F2} kg");
    public override void BuildFooter() => _report.AddSection($"Generated at: {System.DateTime.UtcNow:O}");

    public override WarehouseReport GetResult()
    {
        _report.Summary = $"Inventory Report with {_report.Sections.Count} sections";
        return _report;
    }
}

public class ReportDirector
{
    public void Construct(ReportBuilder builder, int count, double weight)
    {
        builder.BuildHeader();
        builder.BuildStatistics(count, weight);
        builder.BuildFooter();
    }
}
