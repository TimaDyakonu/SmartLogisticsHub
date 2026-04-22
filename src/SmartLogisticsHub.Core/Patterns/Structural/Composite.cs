using System.Collections.Generic;
using System.Linq;
using SmartLogisticsHub.Core.Models;

namespace SmartLogisticsHub.Core.Patterns.Structural;

public abstract class CargoComponent
{
    public abstract string Name { get; }
    public abstract double GetTotalWeight();
}

public class SingleCargoItem : CargoComponent
{
    private readonly string _name;
    private readonly double _weight;

    public SingleCargoItem(string name, double weight)
    {
        _name = name;
        _weight = weight;
    }

    public override string Name => _name;
    public override double GetTotalWeight() => _weight;
}

public class CargoBundle : CargoComponent
{
    private readonly string _bundleName;
    private readonly List<CargoComponent> _children = new List<CargoComponent>();

    public CargoBundle(string name) => _bundleName = name;

    public void Add(CargoComponent component) => _children.Add(component);

    public override string Name => _bundleName;
    public override double GetTotalWeight() => _children.Sum(c => c.GetTotalWeight());

    public static CargoComponent FromEntity(CargoEntity entity)
    {
        if (!entity.IsBundle)
        {
            return new SingleCargoItem(entity.Name, entity.Weight);
        }

        var bundle = new CargoBundle(entity.Name);
        foreach (var child in entity.Children)
        {
            bundle.Add(FromEntity(child));
        }
        return bundle;
    }
}
