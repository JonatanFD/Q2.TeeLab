using Q2.TeeLab.Shared.Domain.Repositories;
using Q2.TeeLab.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace Q2.TeeLab.Shared.Infrastructure.Persistence.EFC.Repositories;

/// <summary>
///     Unit of work implementation
/// </summary>
/// <remarks>
///     This class implements the basic operations for a unit of work.
///     It requires the context to be passed in the constructor.
/// </remarks>
/// <see cref="IUnitOfWork" />
public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    /// <inheritdoc />
    public async Task CompleteAsync()
    {
        await context.SaveChangesAsync();
    }
}