using Q2.TeeLab.DesignLab.Domain.Model.ValueObjects;

namespace Q2.TeeLab.DesignLab.Domain.Model.Commands;

public record SendLayerToBackInProjectCommand(ProjectId projectId, LayerId layerId);