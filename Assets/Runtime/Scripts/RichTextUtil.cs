using UnityEngine.UI;
using UnityEngine;

public struct ColorfulText
{
    public string Text { get; private set; }
    public Color Color { get; private set; }
    public ColorfulText(string text, Color color)
    {
        Text = text;
        Color = color;
    }
}

public static class RichTextUtil
{
    public static string GetColorfulText(string text, Color color)
    {
        return $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{text}</color>";
    }

    public static string GetColorfulText(params ColorfulText[] colorfulText)
    {
        string result = string.Empty;
        foreach (var item in colorfulText)
        {
            result += GetColorfulText(item.Text, item.Color);
        }
        return result;
    }
}
