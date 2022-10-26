using OpenTK.Graphics.OpenGL;
using SFGraphicsEngine.Core.Rendering.Shading.Exceptions;

namespace SFGraphicsEngine.Core.Rendering.Shading
{
    public class Shader : IDisposable
    {
        public int Handle { get; set; }

        public Shader(string source,ShaderType type)
        {
            Handle = GL.CreateShader(type);

            GL.ShaderSource(Handle, source);
            GL.CompileShader(Handle);

            string compilationLog = GL.GetShaderInfoLog(Handle);
            if(compilationLog != "") throw new ShaderCompilationException(compilationLog);
        }

        public void Dispose()
        {
            GL.DeleteShader(Handle);
        }
    }
}
