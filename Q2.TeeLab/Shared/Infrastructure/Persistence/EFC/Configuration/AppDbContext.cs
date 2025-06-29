using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using Microsoft.EntityFrameworkCore;
using Q2.TeeLab.DesignLab.Infrastructure.Persistence.EFC.Configuration.Extensions;
using Q2.TeeLab.OrderProcessing.Domain.Model.Aggregates;
using Q2.TeeLab.OrderProcessing.Domain.Model.Entities;
using Q2.TeeLab.OrderProcessing.Infrastructure.Persistence.EFC.Configuration.Extensions;
using Q2.TeeLab.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;

namespace Q2.TeeLab.Shared.Infrastructure.Persistence.EFC.Configuration;


/// <summary>
///     Application database context
/// </summary>
public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    // Order Processing DbSets
    public DbSet<Order> Orders { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        // Add the created and updated interceptor
        builder.AddCreatedUpdatedInterceptor();
        base.OnConfiguring(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Design Lab Context
        builder.ApplyDesignLabConfiguration();

        // Order Processing Context
        builder.ApplyOrderProcessingConfiguration();

        builder.UseSnakeCaseNamingConvention();
    }
}