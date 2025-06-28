namespace Q2.TeeLab.DesignLab.Interfaces.REST.Resources;

public class LayerResource
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }
    public string Type { get; set; } = string.Empty;
    public float Opacity { get; set; }
    public bool IsVisible { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class TextLayerResource : LayerResource
{
    public string Text { get; set; } = string.Empty;
    public int FontSize { get; set; }
    public string FontColor { get; set; } = string.Empty;
    public string FontFamily { get; set; } = string.Empty;
    public bool IsBold { get; set; }
    public bool IsUnderlined { get; set; }
    public bool IsItalic { get; set; }
}

public class ImageLayerResource : LayerResource
{
    public string ImageUrl { get; set; } = string.Empty;
    public float Width { get; set; }
    public float Height { get; set; }
}