using OpenTK.Graphics.OpenGL;

namespace SFGraphicsEngine.Core.Rendering
{
    /// <summary>
    /// Renderable object
    /// </summary>
    public class VertexArrayObject : IDisposable
    {
        public int Id { get; set; }

        private int NumOfIndices { get; set; }
        public BufferObject<float> Vertices { get; set; }
        public BufferObject<float> Uvs { get; set; }
        public VertexArrayObject(BufferObject<float> vertices, BufferObject<float> uvs)
        {
            Vertices = vertices;
            Uvs = uvs;
            NumOfIndices = vertices.Length;

            Id = GL.GenVertexArray();

            GL.BindVertexArray(Id);

            vertices.Bind();
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            uvs.Bind();
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(1);

            GL.BindVertexArray(0);
        }

        public void Bind() => GL.BindVertexArray(Id);

        public void Unbind() => GL.BindVertexArray(0);

        public void Render()
        {
            Bind();
            GL.DrawArrays(PrimitiveType.Triangles,0, Vertices.Length);
            Unbind();
        }

        public void Dispose()
        {
            Vertices.Dispose();
            Uvs.Dispose();

            GL.DeleteVertexArray(Id);
        }
    }
}
