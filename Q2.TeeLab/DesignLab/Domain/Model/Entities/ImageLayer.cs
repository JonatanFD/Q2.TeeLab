using Q2.TeeLab.DesignLab.Domain.Model.ValueObjects;

namespace Q2.TeeLab.DesignLab.Domain.Model.Entities;

public class ImageLayer : Layer
{
    public Uri ImageUrl { get; private set; }
    public float Width { get; private set; }
    public float Height { get; private set; }

    public ImageLayer(string imageUrl, float width, float height, int z) : base(ELayerType.Image, z)
    {
        ImageUrl = new Uri(imageUrl);
        Width = width;
        Height = height;
    }
}