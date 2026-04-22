using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartLogisticsHub.Core.Models;
using SmartLogisticsHub.Infrastructure.Auth;

namespace SmartLogisticsHub.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Item> Items { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<CargoEntity> CargoEntities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<CargoEntity>()
            .HasMany(c => c.Children)
            .WithOne()
            .HasForeignKey(c => c.ParentId);
    }
}
