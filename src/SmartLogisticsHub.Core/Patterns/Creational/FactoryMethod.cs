namespace SmartLogisticsHub.Core.Patterns.Creational;

public abstract class WarehouseDocument { public abstract string Print(); }
public class DeliveryNote : WarehouseDocument { public override string Print() => "Printing Delivery Note: List of Items for Customer"; }
public class Waybill : WarehouseDocument { public override string Print() => "Printing Waybill: Carrier Routing Info"; }

public abstract class DocumentCreator
{
    public abstract WarehouseDocument FactoryMethod();
    public string CreateAndPrint() => $"Success: {FactoryMethod().Print()}";
}

public class DeliveryNoteCreator : DocumentCreator
{
    public override WarehouseDocument FactoryMethod() => new DeliveryNote();
}

public class WaybillCreator : DocumentCreator
{
    public override WarehouseDocument FactoryMethod() => new Waybill();
}
