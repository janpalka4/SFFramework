

using OpenTK.Mathematics;

namespace SFGraphicsEngine.Core.Rendering.Shading
{
    public static class GlobalUniforms
    {
        public static Matrix4 Projection = Matrix4.CreateOrthographic(800, 600, .1f, 100f);
        public static Matrix4 View = Matrix4.CreateTranslation(Vector3.Zero - Vector3.UnitZ);
        public static Matrix4 Model = Matrix4.CreateTranslation(Vector3.Zero);
    }
}
