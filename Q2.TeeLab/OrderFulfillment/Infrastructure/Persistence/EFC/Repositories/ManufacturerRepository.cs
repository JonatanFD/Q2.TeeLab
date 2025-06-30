using Microsoft.EntityFrameworkCore;
using Q2.TeeLab.OrderFulfillment.Domain.Model.Aggregates;
using Q2.TeeLab.OrderFulfillment.Domain.Model.ValueObjects;
using Q2.TeeLab.OrderFulfillment.Domain.Repositories;
using Q2.TeeLab.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace Q2.TeeLab.OrderFulfillment.Infrastructure.Persistence.EFC.Repositories;

public class ManufacturerRepository : IManufacturerRepository
{
    private readonly AppDbContext context;

    public ManufacturerRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<Manufacturer?> FindByIdAsync(ManufacturerId id)
    {
        return await context.Set<Manufacturer>().FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Manufacturer>> FindAllAsync()
    {
        return await context.Set<Manufacturer>().ToListAsync();
    }

    public async Task<IEnumerable<Manufacturer>> FindActiveManufacturersAsync()
    {
        return await context.Set<Manufacturer>()
            .Where(m => m.IsActive)
            .ToListAsync();
    }

    public async Task<Manufacturer?> FindByTaxIdentificationNumberAsync(string taxIdentificationNumber)
    {
        return await context.Set<Manufacturer>()
            .FirstOrDefaultAsync(m => m.TaxIdentificationNumber == taxIdentificationNumber);
    }

    public async Task<Manufacturer?> FindByEmailAsync(string email)
    {
        return await context.Set<Manufacturer>()
            .FirstOrDefaultAsync(m => m.Email == email);
    }

    public async Task<IEnumerable<Manufacturer>> FindBySpecializationAsync(string specialization)
    {
        return await context.Set<Manufacturer>()
            .Where(m => m.Specialization != null && m.Specialization.Contains(specialization) && m.IsActive)
            .ToListAsync();
    }

    public async Task SaveAsync(Manufacturer manufacturer)
    {
        var existingManufacturer = await FindByIdAsync(manufacturer.Id);
        if (existingManufacturer == null)
        {
            await context.Set<Manufacturer>().AddAsync(manufacturer);
        }
        else
        {
            context.Set<Manufacturer>().Update(manufacturer);
        }
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ManufacturerId id)
    {
        var manufacturer = await FindByIdAsync(id);
        if (manufacturer != null)
        {
            context.Set<Manufacturer>().Remove(manufacturer);
            await context.SaveChangesAsync();
        }
    }
}
