using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Q2.TeeLab.OrderProcessing.Domain.Model.Aggregates;
using Q2.TeeLab.OrderProcessing.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderProcessing.Infrastructure.Persistence.EFC.Configuration;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("carts");
        
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Id)
            .HasConversion(
                cartId => cartId.Value,
                value => new CartId(value))
            .ValueGeneratedNever();

        builder.Property(c => c.UserId)
            .HasConversion(
                userId => userId.Value,
                value => new Shared.Domain.Model.ValueObjects.UserId(value))
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.LastUpdated)
            .IsRequired();

        builder.OwnsOne(c => c.TotalAmount, money =>
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

        builder.HasMany(c => c.Items)
            .WithOne()
            .HasForeignKey("CartId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => c.UserId)
            .IsUnique();
    }
}
