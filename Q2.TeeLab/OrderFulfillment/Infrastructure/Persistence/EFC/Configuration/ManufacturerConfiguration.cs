using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Q2.TeeLab.OrderFulfillment.Domain.Model.Aggregates;
using Q2.TeeLab.OrderFulfillment.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderFulfillment.Infrastructure.Persistence.EFC.Configuration;

public class ManufacturerConfiguration : IEntityTypeConfiguration<Manufacturer>
{
    public void Configure(EntityTypeBuilder<Manufacturer> builder)
    {
        builder.ToTable("Manufacturers");
        
        builder.HasKey(m => m.Id);
        
        builder.Property(m => m.Id)
            .HasConversion(
                manufacturerId => manufacturerId.Value,
                value => new ManufacturerId(value))
            .ValueGeneratedNever();

        builder.Property(m => m.CompanyName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(m => m.ContactPersonName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(m => m.TaxIdentificationNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(m => m.Website)
            .HasMaxLength(200);

        builder.Property(m => m.Specialization)
            .HasMaxLength(500);

        builder.Property(m => m.IsActive)
            .IsRequired();

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        builder.Property(m => m.UpdatedAt)
            .IsRequired();

        // Configure Address as owned type
        builder.OwnsOne(m => m.Address, address =>
        {
            address.Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("AddressStreet");

            address.Property(a => a.City)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("AddressCity");

            address.Property(a => a.State)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("AddressState");

            address.Property(a => a.PostalCode)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("AddressPostalCode");

            address.Property(a => a.Country)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("AddressCountry");
        });

        // Create unique indexes
        builder.HasIndex(m => m.TaxIdentificationNumber)
            .IsUnique();

        builder.HasIndex(m => m.Email)
            .IsUnique();
    }
}
