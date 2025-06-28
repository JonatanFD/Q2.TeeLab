using Microsoft.EntityFrameworkCore;
using Q2.TeeLab.DesignLab.Domain.Model.Entities;
using Q2.TeeLab.DesignLab.Domain.Model.ValueObjects;
using Q2.TeeLab.DesignLab.Domain.Repositories;
using Q2.TeeLab.Shared.Infrastructure.Persistence.EFC.Configuration;
using Q2.TeeLab.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace Q2.TeeLab.DesignLab.Infrastructure.Persistence.EFC.Repositories;

public class LayerRepository(AppDbContext context) : BaseRepository<Layer>(context), ILayerRepository
{
    public async Task<IEnumerable<Layer>> FindByProjectIdAsync(ProjectId projectId)
    {
        return await Context.Set<Layer>()
            .Where(l => l.ProjectId.Id == projectId.Id)
            .OrderBy(l => l.Z)
            .ToListAsync();
    }
}
