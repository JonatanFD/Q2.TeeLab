using Microsoft.EntityFrameworkCore;
using Q2.TeeLab.DesignLab.Domain.Model.Aggregates;
using Q2.TeeLab.DesignLab.Domain.Repositories;
using Q2.TeeLab.Shared.Infrastructure.Persistence.EFC.Configuration;
using Q2.TeeLab.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace Q2.TeeLab.DesignLab.Infrastructure.Persistence.EFC.Repositories;

public class ProjectRepository(AppDbContext context) : BaseRepository<Project>(context), IProjectRepository
{
    public async Task<Project?> FindByIdWithLayersAsync(Guid id)
    {
        return await Context.Set<Project>()
            .Include(p => p.Layers)
            .FirstOrDefaultAsync(p => p.Id.Id == id);
    }

    public async Task<IEnumerable<Project>> FindByUserIdAsync(Guid userId)
    {
        return await Context.Set<Project>()
            .Include(p => p.Layers)
            .Where(p => p.UserId.Id == userId)
            .ToListAsync();
    }
}
