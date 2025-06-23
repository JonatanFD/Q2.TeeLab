using Microsoft.EntityFrameworkCore;
using Q2.TeeLab.DesignLab.Domain.Model.Aggregates;
using Q2.TeeLab.DesignLab.Domain.Model.Entities;
using Q2.TeeLab.DesignLab.Domain.Model.ValueObjects;

namespace Q2.TeeLab.DesignLab.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyDesignLabConfiguration(this ModelBuilder builder)
    {
        ApplyProjectConfiguration(builder);
    }

    private static void ApplyProjectConfiguration(this ModelBuilder builder)
    {
        var entity = builder.Entity<Project>();

        entity.HasKey(p => p.Id);
        entity.Property(p => p.Id)
            .HasConversion(p => p.Id.ToString(), str => new ProjectId(str))
            .IsRequired().ValueGeneratedOnAdd();

        entity.Property(p => p.UserId)
            .IsRequired();

        entity.Property(p => p.Title).IsRequired().HasMaxLength(30);
        entity.Property(p => p.PreviewImageUrl).HasConversion(uri => uri.ToString(), str => new Uri(str));

        entity.Property(p => p.GarmentColor).IsRequired().HasConversion<string>();
        entity.Property(p => p.GarmentGender).IsRequired().HasConversion<string>();
        entity.Property(p => p.GarmentSize).IsRequired().HasConversion<string>();
        entity.HasMany(p => p.Layers)
            .WithOne()
            .HasForeignKey(l => l.ProjectId)
            .IsRequired();

        entity.Property(p => p.CreatedAt).IsRequired().ValueGeneratedOnAdd();
        entity.Property(p => p.UpdatedAt).IsRequired().ValueGeneratedOnAddOrUpdate();
    }

    private static void ApplyLayerConfiguration(this ModelBuilder builder)
    {
        var entity = builder.Entity<Layer>();
        entity.HasDiscriminator(l => l.Type);
        entity.HasDiscriminator<string>("layer_type")
            .HasValue<Layer>("layer_base")
            .HasValue<TextLayer>("layer_text")
            .HasValue<ImageLayer>("layer_image");

        entity.HasKey(l => l.Id);
        entity.Property(l => l.Id)
            .HasConversion(l => l.Id.ToString(), str => new LayerId(str))
            .IsRequired().ValueGeneratedOnAdd();
        
        entity.Property(l => l.ProjectId)
            .HasConversion(p => p.Id.ToString(), str => new ProjectId(str))
            .IsRequired();
        
        entity.Property(l => l.X).IsRequired();
        entity.Property(l => l.Y).IsRequired();
        entity.Property(l => l.Z).IsRequired();
        
        entity.Property(l => l.Opacity).IsRequired();
        entity.Property(l => l.IsVisible).IsRequired();
        
        entity.Property(l => l.CreatedAt).IsRequired().ValueGeneratedOnAdd();
        entity.Property(l => l.UpdatedAt).IsRequired().ValueGeneratedOnAddOrUpdate();
        
        
        
    }
}