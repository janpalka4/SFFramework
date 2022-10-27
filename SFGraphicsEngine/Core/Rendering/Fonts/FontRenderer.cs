using SFGraphicsEngine.Core.Rendering.Shading;

namespace SFGraphicsEngine.Core.Rendering.Fonts
{
    /// <summary>
    /// Manages text render
    /// </summary>
    public class FontRenderer : IDisposable
    {
        const int FontSize = 18;
        public CharAtlas Atlas { get; set; }

        private VertexArrayObject VertexArray { get; set; }
        private ShaderProgram Program { get; set; }
        private List<Text> Texts { get; set; }
        public FontRenderer()
        {
            Atlas = new CharAtlas("Res/Fonts/Lucida Sans Regular.ttf", FontSize);
            Texts = new List<Text>();
            Program = new ShaderProgram(ShaderLoader.LoadVertexShaderFromResources("FontShader"), ShaderLoader.LoadFragmentShaderFromResources("FontShader"));
        }

        /// <summary>
        /// Registers new label to be rendered
        /// </summary>
        /// <param name="text">label to be added</param>
        public void AddText(Text text)
        {
            Texts.Add(text);
            UpdateRenderer();
        }

        /// <summary>
        /// Updates current vao
        /// </summary>
        private void UpdateRenderer()
        {
            List<float> verts = new List<float>();
            List<float> uvs = new List<float>();

            #region Converts text to vertices and uvs
            foreach (Text text in Texts)
            {
                if (text.Content == null) continue;

                float x = text.Position.X;
                float ux = 0;
                float line = text.Position.Y;
                int i = 0;
                int maxSize = Atlas.CharacterInfos.Max(x => x.Size.Y);

                #region Converts character to vertices
                foreach (char ch in text.Content)
                {
                    if (ch == ' ')
                    {
                        x += Atlas.CharacterInfos['-'].Size.X;
                        continue;
                    }
                    if (ch == '\n') { line += FontSize; x = text.Position.X; continue; }

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
                #endregion
            }
            #endregion

            #region Updates VAO
            if (VertexArray is null)
            {
                VertexArray = new VertexArrayObject(new BufferObject<float>(verts.ToArray()), new BufferObject<float>(uvs.ToArray()));
            }
            else
            {
                VertexArray.Vertices.AppendData(verts.ToArray());
                VertexArray.Uvs.AppendData(uvs.ToArray());
            }
            #endregion
        }

        /// <summary>
        /// Renders text on the screen
        /// </summary>
        public void Render()
        {
            Program.Use();
            Atlas.Bind();
            VertexArray.Render();
        }

        public float[] getQuadVerts() => new float[] {-1,-1, -1,1, 1,1,  1,1, 1,-1, -1,-1};

        public float[] getQuadUvs() => new float[] { 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0 };

        /// <summary>
        /// Cleans up VertexArrayObject
        /// </summary>
        public void Dispose()
        {
            VertexArray.Dispose();
        }
    }
}
