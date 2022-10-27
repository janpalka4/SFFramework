using OpenTK.Mathematics;

namespace SFGraphicsEngine.Core.Rendering.Fonts
{
    /// <summary>
    /// A label
    /// </summary>
    public class Text
    {
        /// <summary>
        /// Size of font
        /// </summary>
        public int FontSize { get; set; }

        /// <summary>
        /// Foreground color of label
        /// </summary>
        public Color4 TextColor { get; set; }

        /// <summary>
        /// Background color of label
        /// </summary>
        public Color4 BackroundColor { get; set; }

        /// <summary>
        /// Label position on screen in pixels
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Label text
        /// </summary>
        public string? Content { get; set; }
    }
}
