using Q2.TeeLab.DesignLab.Domain.Model.ValueObjects;

namespace Q2.TeeLab.DesignLab.Domain.Model.Commands;

public record AddImageLayerToProjectCommand(ProjectId projectId, string imageUrl, float width, float height);