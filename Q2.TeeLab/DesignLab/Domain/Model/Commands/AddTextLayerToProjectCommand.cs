using Q2.TeeLab.DesignLab.Domain.Model.ValueObjects;

namespace Q2.TeeLab.DesignLab.Domain.Model.Commands;

public record AddTextLayerToProjectCommand(ProjectId projectId, string text, string fontSize, string fontColor, int fontFamily, bool isBold, bool isUnderlined, bool isItalic);