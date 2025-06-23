using Q2.TeeLab.DesignLab.Domain.Model.Aggregates;
using Q2.TeeLab.DesignLab.Domain.Model.Commands;

namespace Q2.TeeLab.DesignLab.Domain.Services;

public interface IProjectCommandService
{
    Project Handle(CreateProjectCommand command);
    bool Handle(AddImageLayerToProjectCommand command);
    bool Handle(AddTextLayerToProjectCommand command);
}