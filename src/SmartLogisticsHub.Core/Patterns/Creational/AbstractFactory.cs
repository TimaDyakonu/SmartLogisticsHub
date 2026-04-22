namespace SmartLogisticsHub.Core.Patterns.Creational;

public abstract class ShippingLabel { public abstract string GetFormat(); }
public class ExpressLabel : ShippingLabel { public override string GetFormat() => "QR-Coded Thermal Label (Priority)"; }
public class StandardLabel : ShippingLabel { public override string GetFormat() => "Barcode Paper Label (Standard)"; }

public abstract class ShippingManifest { public abstract string GetDetails(); }
public class ExpressManifest : ShippingManifest { public override string GetDetails() => "Air-Carrier Manifest (Express)"; }
public class StandardManifest : ShippingManifest { public override string GetDetails() => "Road-Freight Manifest (Standard)"; }

public abstract class ShippingFamilyFactory
{
    public abstract ShippingLabel CreateLabel();
    public abstract ShippingManifest CreateManifest();
}

public class ExpressShippingFactory : ShippingFamilyFactory
{
    public override ShippingLabel CreateLabel() => new ExpressLabel();
    public override ShippingManifest CreateManifest() => new ExpressManifest();
}

public class StandardShippingFactory : ShippingFamilyFactory
{
    public override ShippingLabel CreateLabel() => new StandardLabel();
    public override ShippingManifest CreateManifest() => new StandardManifest();
}
