using Q2.TeeLab.DesignLab.Domain.Model.Aggregates;
using Q2.TeeLab.Shared.Domain.Repositories;

namespace Q2.TeeLab.DesignLab.Domain.Repositories;

public interface IProjectRepository : IBaseRepository<Project>
{
    Task<Project?> FindByIdWithLayersAsync(Guid id);
    Task<IEnumerable<Project>> FindByUserIdAsync(Guid userId);
}