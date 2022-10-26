using OpenTK.Graphics.OpenGL;
using FreeTypeSharp.Native;
using FreeTypeSharp;
using SFGraphicsEngine.Core.Rendering.Imaging;
using OpenTK.Mathematics;

namespace SFGraphicsEngine.Core.Rendering.Fonts
{
    public class FontRenderer : IDisposable
    {
        const int FontSize = 18;
        public CharAtlas Atlas { get; set; }

        private VertexArrayObject VertexArray { get; set; }
        private List<Text> Texts { get; set; }
        public FontRenderer()
        {
            Atlas = new CharAtlas("ArialTh",FontSize);
            Texts = new List<Text>();
        }

        public void AddText(Text text)
        {
            Texts.Add(text);
            UpdateRenderer();
        }

        private void UpdateRenderer()
        {
            foreach (Text text in Texts)
            {
                float x = text.Position.X;
                float ux = 0;
                float line = text.Position.Y;
                int i = 0;
                int maxSize = Atlas.CharacterInfos.Max(x => x.Size.Y);

                List<float> verts = new List<float>();
                List<float> uvs = new List<float>();

                foreach (char ch in text.Content)
                {
                    if (ch == ' ')
                    {
                        x += Atlas.CharacterInfos['-'].Size.X;
                        continue;
                    }
                    if (ch == '\n') { line += FontSize; x = text.Position.X; continue; }

                    //CharObjects.Add(new CharObject() {Color = Color4.White,FontSize = FontSize,Offset=i,Character=ch});

                    CharacterInfo info = Atlas.CharacterInfos[ch];
                    ux = info.AtlasPosition.X;

                    float[] vs = getQuadVerts().Select(x => x == -1 ? 0f : 1f).ToArray();
                    for (int v = 0; v < vs.Length; v += 2)
                    {
                        verts.Add(vs[v] * (info.Size.X) + x);
                        verts.Add(vs[v + 1] * maxSize - (info.Bearing.Y - info.Size.Y) + line);
                    }

                    float[] uv = getQuadUvs();
                    for (int u = 0; u < uv.Length; u += 2)
                    {
                        uvs.Add(uv[u] * info.AtlasSize.X + ux);
                        uvs.Add(uv[u + 1]);
                    }

                    x += info.Size.X + info.Bearing.X;
                    i++;
                }

                if (VertexArray is null)
                {
                    VertexArray = new VertexArrayObject(new BufferObject<float>(verts.ToArray()), new BufferObject<float>(uvs.ToArray()));
                }
                else
                {
                    VertexArray.Vertices.UpdateData(verts.ToArray());
                    VertexArray.Uvs.UpdateData(uvs.ToArray());
                }
            }
        }

        public void Render()
        {
            VertexArray.Render();
        }

        public float[] getQuadVerts() => new float[] {-1,-1, -1,1, 1,1,  1,1, 1,-1, -1,-1};

        public float[] getQuadUvs() => new float[] { 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0 };

        public void Dispose()
        {
            VertexArray.Dispose();
        }
    }
}
