using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Q2.TeeLab.OrderProcessing.Domain.Model.Aggregates;
using Q2.TeeLab.OrderProcessing.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderProcessing.Infrastructure.Persistence.EFC.Configuration;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");
        
        builder.HasKey(o => o.Id);
        
        builder.Property(o => o.Id)
            .HasConversion(
                orderId => orderId.Value,
                value => new OrderId(value))
            .ValueGeneratedNever();

        builder.Property(o => o.UserId)
            .HasConversion(
                userId => userId.Value,
                value => new Shared.Domain.Model.ValueObjects.UserId(value))
            .IsRequired();

        builder.Property(o => o.OrderDate)
            .IsRequired();

        builder.Property(o => o.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.OwnsOne(o => o.TotalAmount, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("total_amount")
                .HasPrecision(18, 2)
                .IsRequired();
                
            money.Property(m => m.Currency)
                .HasColumnName("total_currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.OwnsOne(o => o.DiscountAmount, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("discount_amount")
                .HasPrecision(18, 2)
                .IsRequired();
                
            money.Property(m => m.Currency)
                .HasColumnName("discount_currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.OwnsOne(o => o.FinalAmount, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("final_amount")
                .HasPrecision(18, 2)
                .IsRequired();
                
            money.Property(m => m.Currency)
                .HasColumnName("final_currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.Property(o => o.Notes)
            .HasMaxLength(1000);

        builder.Property(o => o.DeliveryDate);

        builder.Property(o => o.TrackingNumber)
            .HasMaxLength(100);

        builder.HasMany(o => o.Items)
            .WithOne()
            .HasForeignKey("OrderId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(o => o.UserId);
        builder.HasIndex(o => o.Status);
        builder.HasIndex(o => o.OrderDate);
    }
}
