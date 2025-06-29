using Microsoft.EntityFrameworkCore;
using Q2.TeeLab.OrderProcessing.Infrastructure.Persistence.EFC.Configuration;

namespace Q2.TeeLab.OrderProcessing.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyOrderProcessingConfiguration(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new OrderConfiguration());
        builder.ApplyConfiguration(new CartConfiguration());
        builder.ApplyConfiguration(new OrderItemConfiguration());
    }
}
