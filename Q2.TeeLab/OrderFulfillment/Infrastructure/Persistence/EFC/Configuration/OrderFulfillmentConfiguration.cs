using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Q2.TeeLab.OrderFulfillment.Domain.Model.ValueObjects;
using Q2.TeeLab.Shared.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderFulfillment.Infrastructure.Persistence.EFC.Configuration;

public class OrderFulfillmentConfiguration : IEntityTypeConfiguration<Domain.Model.Aggregates.OrderFulfillment>
{
    public void Configure(EntityTypeBuilder<Domain.Model.Aggregates.OrderFulfillment> builder)
    {
        builder.ToTable("OrderFulfillments");
        
        builder.HasKey(of => of.Id);
        
        builder.Property(of => of.Id)
            .HasConversion(
                orderFulfillmentId => orderFulfillmentId.Value,
                value => new OrderFulfillmentId(value))
            .ValueGeneratedNever();

        builder.Property(of => of.OrderId)
            .IsRequired();

        builder.Property(of => of.CustomerId)
            .HasConversion(
                userId => userId.Value,
                value => new UserId(value))
            .IsRequired();

        builder.Property(of => of.ManufacturerId)
            .HasConversion(
                manufacturerId => manufacturerId.Value,
                value => new ManufacturerId(value))
            .IsRequired();

        builder.Property(of => of.ProjectName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(of => of.ProjectDescription)
            .HasMaxLength(1000);

        builder.Property(of => of.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(of => of.OrderDate)
            .IsRequired();

        builder.Property(of => of.EstimatedDeliveryDate);

        builder.Property(of => of.ActualDeliveryDate);

        builder.Property(of => of.TotalAmount)
            .HasPrecision(18, 2);

        builder.Property(of => of.SpecialInstructions)
            .HasMaxLength(1000);

        builder.Property(of => of.CreatedAt)
            .IsRequired();

        builder.Property(of => of.UpdatedAt)
            .IsRequired();

        // Configure relationship with OrderFulfillmentItems
        builder.HasMany(of => of.Items)
            .WithOne()
            .HasForeignKey("OrderFulfillmentId")
            .OnDelete(DeleteBehavior.Cascade);

        // Create indexes
        builder.HasIndex(of => of.OrderId)
            .IsUnique();

        builder.HasIndex(of => of.CustomerId);
        builder.HasIndex(of => of.ManufacturerId);
        builder.HasIndex(of => of.Status);
        builder.HasIndex(of => of.EstimatedDeliveryDate);
    }
}
