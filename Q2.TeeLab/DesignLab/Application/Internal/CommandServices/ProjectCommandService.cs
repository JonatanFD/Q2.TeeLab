using Q2.TeeLab.DesignLab.Domain.Model.Aggregates;
using Q2.TeeLab.DesignLab.Domain.Model.Commands;
using Q2.TeeLab.DesignLab.Domain.Model.ValueObjects;
using Q2.TeeLab.DesignLab.Domain.Repositories;
using Q2.TeeLab.DesignLab.Domain.Services;
using Q2.TeeLab.Shared.Domain.Repositories;

namespace Q2.TeeLab.DesignLab.Application.Internal.CommandServices;

public class ProjectCommandService(IProjectRepository projectRepository, IUnitOfWork unitOfWork) : IProjectCommandService
{
    public async Task<Project> Handle(CreateProjectCommand command)
    {
        var project = new Project(command);
        await projectRepository.AddAsync(project);
        await unitOfWork.CompleteAsync();

        return project;
    }

    public async Task<LayerId> Handle(AddImageLayerToProjectCommand command)
    {
        var project = await projectRepository.FindByIdAsync(command.projectId.Id);
        throw new NotImplementedException();
    }

    public Task<LayerId> Handle(AddTextLayerToProjectCommand command)
    {
        throw new NotImplementedException();
    }
}