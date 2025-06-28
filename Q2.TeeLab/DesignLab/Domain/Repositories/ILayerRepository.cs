using Q2.TeeLab.DesignLab.Domain.Model.Entities;
using Q2.TeeLab.DesignLab.Domain.Model.ValueObjects;
using Q2.TeeLab.Shared.Domain.Repositories;

namespace Q2.TeeLab.DesignLab.Domain.Repositories;

public interface ILayerRepository : IBaseRepository<Layer>
{
    Task<IEnumerable<Layer>> FindByProjectIdAsync(ProjectId projectId);
}