using SmartLogisticsHub.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace SmartLogisticsHub.Core.Patterns.Behavioral;

// Iterator
public interface IIterator<T>
{
    bool HasNext();
    T Next();
}

// IAggregate
public interface IAggregate<T>
{
    IIterator<T> CreateIterator();
}

// Concrete Iterator
public class HeavyItemIterator : IIterator<Item>
{
    private readonly List<Item> _collection;
    private int _index = 0;

    public HeavyItemIterator(IEnumerable<Item> items)
    {
        _collection = items.Where(i => i.Weight > 50).ToList();
    }

    public bool HasNext() => _index < _collection.Count;
    
    public Item Next() => _collection[_index++];
}

// Concrete Aggregate
public class ItemAggregate : IAggregate<Item>
{
    private readonly List<Item> _items;

    public ItemAggregate(IEnumerable<Item> items) => _items = items.ToList();

    public IIterator<Item> CreateIterator() => new HeavyItemIterator(_items);
}