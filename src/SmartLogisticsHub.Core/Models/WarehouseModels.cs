using System;
using System.Collections.Generic;
using SmartLogisticsHub.Core.Abstractions;

namespace SmartLogisticsHub.Core.Models;

public class Item : IPrototype<Item>
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string SKU { get; set; }
    public double Weight { get; set; }
    public double Volume { get; set; }
    public string Category { get; set; }

    public Item Clone() => (Item)this.MemberwiseClone();
}

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string CustomerName { get; set; }
    public string Status { get; set; }
}

public class CargoEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public double Weight { get; set; }
    public bool IsBundle { get; set; }
    public Guid? ParentId { get; set; }
    public virtual ICollection<CargoEntity> Children { get; set; } = new List<CargoEntity>();
}
