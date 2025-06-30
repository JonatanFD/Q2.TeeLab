using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Q2.TeeLab.OrderFulfillment.Domain.Model.Entities;
using Q2.TeeLab.OrderFulfillment.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderFulfillment.Infrastructure.Persistence.EFC.Configuration;

public class OrderFulfillmentItemConfiguration : IEntityTypeConfiguration<OrderFulfillmentItem>
{
    public void Configure(EntityTypeBuilder<OrderFulfillmentItem> builder)
    {
        builder.ToTable("OrderFulfillmentItems");
        
        builder.HasKey(ofi => ofi.Id);
        
        builder.Property(ofi => ofi.Id)
            .HasConversion(
                orderFulfillmentItemId => orderFulfillmentItemId.Value,
                value => new OrderFulfillmentItemId(value))
            .ValueGeneratedNever();

        builder.Property(ofi => ofi.ProductId)
            .HasConversion(
                productId => productId.Value,
                value => new ProductId(value))
            .IsRequired();

        builder.Property(ofi => ofi.ProductName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(ofi => ofi.Quantity)
            .IsRequired();

        builder.Property(ofi => ofi.Progress)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(ofi => ofi.ProgressNotes)
            .HasMaxLength(1000);

        builder.Property(ofi => ofi.StartDate);

        builder.Property(ofi => ofi.EstimatedCompletionDate);

        builder.Property(ofi => ofi.ActualCompletionDate);

        builder.Property(ofi => ofi.CreatedAt)
            .IsRequired();

        builder.Property(ofi => ofi.UpdatedAt)
            .IsRequired();

        // Create indexes
        builder.HasIndex(ofi => ofi.ProductId);
        builder.HasIndex(ofi => ofi.Progress);
    }
}
