using SFGraphicsEngine.Core.Rendering.Fonts;
using System.Linq;

namespace SFGraphicsEngine.Core.Rendering
{
    /// <summary>
    /// Used to manage scene render
    /// </summary>
    public class RenderContext : IDisposable
    {
        public FontRenderer FontRenderer { get; set; }

        private List<VertexArrayObject> VertexArrayObjects { get; set; }

        public RenderContext()
        {
            VertexArrayObjects = new List<VertexArrayObject>();
            FontRenderer = new FontRenderer();
        }

        public void AddVAO(VertexArrayObject VAO) => VertexArrayObjects.Add(VAO);

        public VertexArrayObject? GetVAO(VertexArrayObject VAO) => VertexArrayObjects.FirstOrDefault(x => x == VAO);

        public VertexArrayObject GetVAO(int index) => VertexArrayObjects[index];

        public VertexArrayObject? GetVAObyId(int Id) => VertexArrayObjects.FirstOrDefault(x => x.Id == Id);

        public void Render()
        {
            foreach (VertexArrayObject VAO in VertexArrayObjects)
                VAO.Render();

            FontRenderer.Render();
        }

        public void Dispose()
        {
            foreach (VertexArrayObject VAO in VertexArrayObjects)
                VAO.Dispose();

            FontRenderer.Dispose();
        }
    }
}
