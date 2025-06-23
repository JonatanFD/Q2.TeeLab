using Q2.TeeLab.DesignLab.Domain.Model.Aggregates;
using Q2.TeeLab.DesignLab.Domain.Model.Queries;
using Q2.TeeLab.DesignLab.Domain.Services;

namespace Q2.TeeLab.DesignLab.Application.Internal.QueryServices;

public class ProjectQueryService: IProjectQueryService
{
    public Task<Project> Handle(GetProjectByIdQuery query)
    {
        throw new NotImplementedException();
    }

    public Task<List<Project>> Handle(GetProjectsByUserIdQuery query)
    {
        throw new NotImplementedException();
    }
}