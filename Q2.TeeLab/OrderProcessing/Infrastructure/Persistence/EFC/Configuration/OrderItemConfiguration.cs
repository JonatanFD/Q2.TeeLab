using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Q2.TeeLab.OrderProcessing.Domain.Model.Entities;
using Q2.TeeLab.OrderProcessing.Domain.Model.ValueObjects;
using System.Text.Json;

namespace Q2.TeeLab.OrderProcessing.Infrastructure.Persistence.EFC.Configuration;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("order_items");
        
        builder.HasKey(oi => oi.Id);
        
        builder.Property(oi => oi.Id)
            .HasConversion(
                itemId => itemId.Value,
                value => new OrderItemId(value))
            .ValueGeneratedNever();

        builder.Property(oi => oi.Quantity)
            .IsRequired();

        builder.OwnsOne(oi => oi.UnitPrice, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("unit_price")
                .HasPrecision(18, 2)
                .IsRequired();
                
            money.Property(m => m.Currency)
                .HasColumnName("unit_currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.OwnsOne(oi => oi.TotalPrice, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("total_price")
                .HasPrecision(18, 2)
                .IsRequired();
                
            money.Property(m => m.Currency)
                .HasColumnName("total_currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        // Configure ProductInfo as owned entity
        builder.OwnsOne(oi => oi.Product, product =>
        {
            product.Property(p => p.Id)
                .HasConversion(
                    productId => productId.Value,
                    value => new ProductId(value))
                .HasColumnName("product_id")
                .IsRequired();

            product.Property(p => p.ProjectId)
                .HasConversion(
                    projectId => projectId.Value,
                    value => new ProjectId(value))
                .HasColumnName("project_id")
                .IsRequired();

            product.Property(p => p.Description)
                .HasColumnName("product_description")
                .HasMaxLength(500)
                .IsRequired();

            product.OwnsOne(p => p.Price, money =>
            {
                money.Property(m => m.Amount)
                    .HasColumnName("product_price")
                    .HasPrecision(18, 2)
                    .IsRequired();
                    
                money.Property(m => m.Currency)
                    .HasColumnName("product_currency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

            // Store discounts as JSON
            product.Property(p => p.Discounts)
                .HasColumnName("product_discounts")
                .HasConversion(
                    discounts => JsonSerializer.Serialize(discounts, (JsonSerializerOptions?)null),
                    json => JsonSerializer.Deserialize<List<Discount>>(json, (JsonSerializerOptions?)null) ?? new List<Discount>())
                .HasColumnType("json");
        });

        // Store applied discounts as JSON
        builder.Property(oi => oi.AppliedDiscounts)
            .HasColumnName("applied_discounts")
            .HasConversion(
                discounts => JsonSerializer.Serialize(discounts, (JsonSerializerOptions?)null),
                json => JsonSerializer.Deserialize<List<Discount>>(json, (JsonSerializerOptions?)null) ?? new List<Discount>())
            .HasColumnType("json");

        // Foreign key will be added automatically by EF based on the navigation property
        builder.Property<Guid>("OrderId");
        builder.Property<Guid?>("CartId");

        builder.HasIndex("OrderId");
        builder.HasIndex("CartId");
    }
}
