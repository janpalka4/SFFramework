using OpenTK.Mathematics;

namespace SFGraphicsEngine.Core.Rendering.Fonts
{
    public class Text
    {
        public int FontSize { get; set; }
        public Color4 TextColor { get; set; }
        public Color4 BackroundColor { get; set; }
        public Vector2 Position { get; set; }
        public string? Content { get; set; }
    }
}
