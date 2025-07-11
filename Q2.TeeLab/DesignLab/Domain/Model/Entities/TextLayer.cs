﻿using Q2.TeeLab.DesignLab.Domain.Model.ValueObjects;

namespace Q2.TeeLab.DesignLab.Domain.Model.Entities;

public class TextLayer : Layer
{
    public string Text { get; private set; }
    public int FontSize { get; private set; }
    public string FontColor { get; private set; }
    public string FontFamily { get; private set; }
    public bool IsBold { get; private set; }
    public bool IsUnderlined { get; private set; }
    public bool IsItalic { get; private set; }
    
    public TextLayer(ProjectId projectId, string text, string fontFamily, int fontSize, string fontColor, bool isBold, bool isUnderlined, bool isItalic, int z) : base(projectId, ELayerType.Text, z)
    {
        Text = text;
        FontSize = fontSize;
        FontColor = fontColor;
        FontFamily = fontFamily;
        IsBold = isBold;
        IsUnderlined = isUnderlined;
        IsItalic = isItalic;
    }
}