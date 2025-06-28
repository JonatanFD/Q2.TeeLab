using Q2.TeeLab.DesignLab.Domain.Model.Entities;
using Q2.TeeLab.DesignLab.Interfaces.REST.Resources;

namespace Q2.TeeLab.DesignLab.Interfaces.REST.Transform;

public static class LayerResourceFromEntityAssembler
{
    public static LayerResource ToResource(Layer layer)
    {
        return layer switch
        {
            TextLayer textLayer => new TextLayerResource
            {
                Id = textLayer.Id.Id,
                ProjectId = textLayer.ProjectId.Id,
                X = textLayer.X,
                Y = textLayer.Y,
                Z = textLayer.Z,
                Type = textLayer.Type.ToString(),
                Opacity = textLayer.Opacity,
                IsVisible = textLayer.IsVisible,
                CreatedAt = textLayer.CreatedAt,
                UpdatedAt = textLayer.UpdatedAt,
                Text = textLayer.Text,
                FontSize = textLayer.FontSize,
                FontColor = textLayer.FontColor,
                FontFamily = textLayer.FontFamily,
                IsBold = textLayer.IsBold,
                IsUnderlined = textLayer.IsUnderlined,
                IsItalic = textLayer.IsItalic
            },
            ImageLayer imageLayer => new ImageLayerResource
            {
                Id = imageLayer.Id.Id,
                ProjectId = imageLayer.ProjectId.Id,
                X = imageLayer.X,
                Y = imageLayer.Y,
                Z = imageLayer.Z,
                Type = imageLayer.Type.ToString(),
                Opacity = imageLayer.Opacity,
                IsVisible = imageLayer.IsVisible,
                CreatedAt = imageLayer.CreatedAt,
                UpdatedAt = imageLayer.UpdatedAt,
                ImageUrl = imageLayer.ImageUrl.ToString(),
                Width = imageLayer.Width,
                Height = imageLayer.Height
            },
            _ => new LayerResource
            {
                Id = layer.Id.Id,
                ProjectId = layer.ProjectId.Id,
                X = layer.X,
                Y = layer.Y,
                Z = layer.Z,
                Type = layer.Type.ToString(),
                Opacity = layer.Opacity,
                IsVisible = layer.IsVisible,
                CreatedAt = layer.CreatedAt,
                UpdatedAt = layer.UpdatedAt
            }
        };
    }
}
