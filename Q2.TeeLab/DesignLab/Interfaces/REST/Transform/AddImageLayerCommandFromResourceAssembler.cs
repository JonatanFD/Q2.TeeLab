using Q2.TeeLab.DesignLab.Domain.Model.Commands;
using Q2.TeeLab.DesignLab.Domain.Model.ValueObjects;
using Q2.TeeLab.DesignLab.Interfaces.REST.Resources;

namespace Q2.TeeLab.DesignLab.Interfaces.REST.Transform;

public static class AddImageLayerCommandFromResourceAssembler
{
    public static AddImageLayerToProjectCommand ToCommand(Guid projectId, AddImageLayerResource resource)
    {
        return new AddImageLayerToProjectCommand(
            new ProjectId(projectId),
            resource.ImageUrl,
            resource.Width,
            resource.Height
        );
    }
}
