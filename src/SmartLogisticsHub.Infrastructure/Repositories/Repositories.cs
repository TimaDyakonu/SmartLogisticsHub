using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartLogisticsHub.Core.Abstractions;
using SmartLogisticsHub.Core.Models;
using SmartLogisticsHub.Core.Patterns.Structural;
using SmartLogisticsHub.Infrastructure.Data;

namespace SmartLogisticsHub.Infrastructure.Repositories;

public class ItemRepository : IItemRepository
{
    private readonly AppDbContext _context;
    public ItemRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Item>> GetAllAsync() => await _context.Items.ToListAsync();
    public async Task<Item?> GetByIdAsync(Guid id) => await _context.Items.FindAsync(id);
    public async Task AddAsync(Item item) => await _context.Items.AddAsync(item);
    public async Task UpdateAsync(Item item) => _context.Items.Update(item);
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    public async Task<int> CountAsync() => await _context.Items.CountAsync();

    public async Task<double> TotalWeightAsync() => await _context.Items.SumAsync(i => i.Weight);
}

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;
    public OrderRepository(AppDbContext context) => _context = context;

    public async Task<Order?> GetByIdAsync(Guid id) => await _context.Orders.FindAsync(id);
    public async Task UpdateAsync(Order order) => _context.Orders.Update(order);
    public async Task<IEnumerable<Order>> GetAllAsync() => await _context.Orders.ToListAsync();
    public async Task AddAsync(Order order) => await _context.Orders.AddAsync(order);
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}

public class CargoRepository : ICargoRepository
{
    private readonly AppDbContext _context;
    public CargoRepository(AppDbContext context) => _context = context;
    
    public async Task<CargoEntity?> GetRootWithChildrenAsync(Guid id)
    {
        var root = await _context.CargoEntities
            .FirstOrDefaultAsync(x => x.Id == id);

        if (root == null)
            return null;

        await LoadChildrenRecursive(root);
        return root;
    }

    public async Task AddAsync(CargoEntity entity) => await _context.CargoEntities.AddAsync(entity);

    private async Task LoadChildrenRecursive(CargoEntity entity, HashSet<Guid>? visited = null)
    {
        visited ??= [];
        
        if (!visited.Add(entity.Id))
            return;

        await _context.Entry(entity)
            .Collection(e => e.Children)
            .LoadAsync();

        foreach (var child in entity.Children)
        {
            await LoadChildrenRecursive(child, visited);
        }
    }

    public async Task UpdateAsync(Order order) => _context.Orders.Update(order);
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}