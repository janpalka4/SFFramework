using OpenTK.Graphics.OpenGL;

namespace SFGraphicsEngine.Core.Rendering.Shading
{
    public class ShaderProgram : IDisposable
    {
        public int ProgramHandle { get; set; }

        private Shader VertexShader { get; set; }
        private Shader FragmentShader { get; set; }
        private int ProjectionLocation { get; set; }
        private int ViewLocation { get; set; }
        private int ModelLocation { get; set; }
        private int ResolutionLocation { get; set; }

        public ShaderProgram(string vertex, string fragment)
        {
            Init(new Shader(vertex, ShaderType.VertexShader), new Shader(fragment, ShaderType.FragmentShader));
        }

        public ShaderProgram(Shader vertex, Shader fragment)
        {
            Init(vertex, fragment);
        }

        private void Init(Shader vertex, Shader fragment)
        {
            ProgramHandle = GL.CreateProgram();

            VertexShader = vertex;
            FragmentShader = fragment;

            GL.AttachShader(ProgramHandle, VertexShader.Handle);
            GL.AttachShader(ProgramHandle, FragmentShader.Handle);

            GL.LinkProgram(ProgramHandle);

            ProjectionLocation = GL.GetUniformLocation(ProgramHandle, "Projection");
            ViewLocation = GL.GetUniformLocation(ProgramHandle, "View");
            ModelLocation = GL.GetUniformLocation(ProgramHandle, "Model");
            ResolutionLocation = GL.GetUniformLocation(ProgramHandle, "Resolution");
        }

        public void Use() 
        {
            GL.UseProgram(ProgramHandle);
            GL.UniformMatrix4(ProjectionLocation, false,ref GlobalUniforms.Projection);
            GL.UniformMatrix4(ViewLocation, false, ref GlobalUniforms.View);
            GL.UniformMatrix4(ModelLocation, false, ref GlobalUniforms.Model);
            GL.Uniform2(ResolutionLocation, 800, 600);
        }

        public void Dispose()
        {
            VertexShader.Dispose();
            FragmentShader.Dispose();
            GL.DeleteProgram(ProgramHandle);
        }
    }
}
