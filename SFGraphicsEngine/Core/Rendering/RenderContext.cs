using System.Linq;

namespace SFGraphicsEngine.Core.Rendering
{
    public class RenderContext : IDisposable
    {
        private List<VertexArrayObject> VertexArrayObjects { get; set; }
        public RenderContext()
        {
            VertexArrayObjects = new List<VertexArrayObject>();
        }

        public void AddVAO(VertexArrayObject VAO) => VertexArrayObjects.Add(VAO);

        public VertexArrayObject? GetVAO(VertexArrayObject VAO) => VertexArrayObjects.FirstOrDefault(x => x == VAO);

        public VertexArrayObject GetVAO(int index) => VertexArrayObjects[index];

        public VertexArrayObject? GetVAObyId(int Id) => VertexArrayObjects.FirstOrDefault(x => x.Id == Id);

        public void Render()
        {
            foreach (VertexArrayObject VAO in VertexArrayObjects)
                VAO.Render();
        }

        public void Dispose()
        {
            foreach (VertexArrayObject VAO in VertexArrayObjects)
                VAO.Dispose();
        }
    }
}
