using Q2.TeeLab.DesignLab.Domain.Model.Aggregates;
using Q2.TeeLab.DesignLab.Domain.Model.Queries;

namespace Q2.TeeLab.DesignLab.Domain.Services;

public interface IProjectQueryService
{
    Project Handle(GetProjectByIdQuery query);
    List<Project> Handle(GetProjectsByUserIdQuery query);
}