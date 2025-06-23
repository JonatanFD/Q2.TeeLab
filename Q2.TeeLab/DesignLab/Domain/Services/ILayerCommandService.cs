using Q2.TeeLab.DesignLab.Domain.Model.Commands;

namespace Q2.TeeLab.DesignLab.Domain.Services;

public interface ILayerCommandService
{
    Task<bool> Handle(SendLayerToBackInProjectCommand command);
}