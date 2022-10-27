using OpenTK.Graphics.OpenGL;
using System.Reflection;

namespace SFGraphicsEngine.Core.Rendering.Shading
{
    /// <summary>
    /// Loads shaders from Assembly
    /// </summary>
    public static class ShaderLoader
    {
        public static Shader LoadVertexShaderFromResources(string ShaderName)
        {
            var assmebly = Assembly.GetExecutingAssembly();

            string resource = assmebly.GetManifestResourceNames().FirstOrDefault(x => x.EndsWith(ShaderName + ".vert"))
                ?? throw new FileNotFoundException();

            Stream stream = assmebly.GetManifestResourceStream(resource);
            StreamReader reader = new StreamReader(stream);

            string source = reader.ReadToEnd();

            return new Shader(source, ShaderType.VertexShader);
        }

        public static Shader LoadFragmentShaderFromResources(string ShaderName)
        {
            var assmebly = Assembly.GetExecutingAssembly();

            string resource = assmebly.GetManifestResourceNames().FirstOrDefault(x => x.EndsWith(ShaderName + ".frag"))
                ?? throw new FileNotFoundException();

            Stream stream = assmebly.GetManifestResourceStream(resource);
            StreamReader reader = new StreamReader(stream);

            string source = reader.ReadToEnd();

            return new Shader(source, ShaderType.FragmentShader);
        }
    }
}
