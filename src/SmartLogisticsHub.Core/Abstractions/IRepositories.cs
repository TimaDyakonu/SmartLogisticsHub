using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartLogisticsHub.Core.Models;
using SmartLogisticsHub.Core.Patterns.Structural;

namespace SmartLogisticsHub.Core.Abstractions;

public interface IItemRepository
{
    Task<IEnumerable<Item>> GetAllAsync();
    Task<Item?> GetByIdAsync(Guid id);
    Task AddAsync(Item item);
    Task UpdateAsync(Item item);
    Task SaveChangesAsync();
    Task<int> CountAsync();
    Task<double> TotalWeightAsync();
}

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task AddAsync(Order order);
    Task<Order?> GetByIdAsync(Guid id);
    Task UpdateAsync(Order order);
    Task SaveChangesAsync();
}

public interface ICargoRepository
{
    Task<CargoEntity?> GetRootWithChildrenAsync(Guid id);
    Task AddAsync(CargoEntity entity);
    Task UpdateAsync(Order order);
    Task SaveChangesAsync();
}
