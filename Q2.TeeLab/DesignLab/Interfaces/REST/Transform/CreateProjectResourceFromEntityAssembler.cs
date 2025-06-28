using Q2.TeeLab.DesignLab.Domain.Model.Aggregates;
using Q2.TeeLab.DesignLab.Interfaces.REST.Resources;

namespace Q2.TeeLab.DesignLab.Interfaces.REST.Transform;

public static class CreateProjectResourceFromEntityAssembler
{
    public static ProjectResource ToResource(Project project)
    {
        return new ProjectResource
        {
            Id = project.Id.Id,
            UserId = project.UserId.Id,
            Title = project.Title,
            PreviewImageUrl = project.PreviewImageUrl?.ToString(),
            GarmentColor = project.GarmentColor.ToString(),
            GarmentGender = project.GarmentGender.ToString(),
            GarmentSize = project.GarmentSize.ToString(),
            Layers = project.Layers.Select(LayerResourceFromEntityAssembler.ToResource),
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt
        };
    }
}