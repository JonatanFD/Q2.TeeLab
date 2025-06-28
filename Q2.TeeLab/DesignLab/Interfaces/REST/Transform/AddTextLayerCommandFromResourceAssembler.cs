using Q2.TeeLab.DesignLab.Domain.Model.Commands;
using Q2.TeeLab.DesignLab.Domain.Model.ValueObjects;
using Q2.TeeLab.DesignLab.Interfaces.REST.Resources;

namespace Q2.TeeLab.DesignLab.Interfaces.REST.Transform;

public static class AddTextLayerCommandFromResourceAssembler
{
    public static AddTextLayerToProjectCommand ToCommand(Guid projectId, AddTextLayerResource resource)
    {
        return new AddTextLayerToProjectCommand(
            new ProjectId(projectId),
            resource.Text,
            resource.FontSize.ToString(),
            resource.FontColor,
            int.Parse(resource.FontFamily),
            resource.IsBold,
            resource.IsUnderlined,
            resource.IsItalic
        );
    }
}
