using Q2.TeeLab.DesignLab.Domain.Model.Aggregates;
using Q2.TeeLab.DesignLab.Domain.Model.Queries;

namespace Q2.TeeLab.DesignLab.Domain.Services;

public interface IProjectQueryService
{
    Task<Project> Handle(GetProjectByIdQuery query);
    Task<List<Project>> Handle(GetProjectsByUserIdQuery query);
}