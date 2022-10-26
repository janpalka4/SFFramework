using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using SFGraphicsEngine.Core;
using SFGraphicsEngine.Core.Rendering;
using SFGraphicsEngine.Core.Rendering.Fonts;
using SFGraphicsEngine.Core.Rendering.Imaging;
using SFGraphicsEngine.Core.Rendering.Shading;
using SFGUIFramework.Core.Shaders;

namespace SFGUIFramework.Core.Windowing
{
    public class Window : SFGraphicsEngine.Core.Window
    {
        private Texture tex;
        private ShaderProgram program;
        private FontRenderer renderer;
        private VertexArrayObject vertexArray;
        public Window() : base()
        {
            WindowInstance.Closed += WindowInstance_Closed;
        }

       
        protected override void WindowInstance_RenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            program.Use();
            tex.Bind();
            renderer.Render();
            //vertexArray.Render();
            WindowInstance.SwapBuffers();
        }

        protected override void WindowInstance_Load()
        {
            base.WindowInstance_Load();
            renderer = new();
            tex = renderer.Atlas;
            float[] verts = { -400, -300, -400, 300, 400, 300, 400, 300, 400, -300, -400, -300 };
            float[] uvs = new float[] { 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0 };

            //vertexArray = new VertexArrayObject(new BufferObject<float>(verts), new BufferObject<float>(uvs));
            /*renderer.DrawText("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Nam libero tempore, cum soluta\n nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat facere\n possimus, omnis voluptas assumenda est, omnis dolor repellendus. Fusce wisi.\n Etiam bibendum elit eget erat. Integer in sapien. Nulla pulvinar eleifend sem. Duis sapien\n nunc, commodo et, interdum suscipit, sollicitudin et, dolor. Integer in sapien. Duis bibendum,\n lectus ut viverra rhoncus, dolor nunc faucibus libero, eget facilisis enim ipsum id lacus.\n Nullam rhoncus aliquam metus.\nLorem ipsum dolor sit amet, consectetuer adipiscing elit. Nam libero tempore, cum soluta\n nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat facere\n possimus, omnis voluptas assumenda est, omnis dolor repellendus. Fusce wisi.\n Etiam bibendum elit eget erat. Integer in sapien. Nulla pulvinar eleifend sem. Duis sapien\n nunc, commodo et, interdum suscipit, sollicitudin et, dolor. Integer in sapien. Duis bibendum,\n lectus ut viverra rhoncus, dolor nunc faucibus libero, eget facilisis enim ipsum id lacus.\n Nullam rhoncus aliquam metus.\nLorem ipsum dolor sit amet, consectetuer adipiscing elit. Nam libero tempore, cum soluta\n nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat facere\n possimus, omnis voluptas assumenda est, omnis dolor repellendus. Fusce wisi.\n Etiam bibendum elit eget erat. Integer in sapien. Nulla pulvinar eleifend sem. Duis sapien\n nunc, commodo et, interdum suscipit, sollicitudin et, dolor. Integer in sapien. Duis bibendum,\n lectus ut viverra rhoncus, dolor nunc faucibus libero, eget facilisis enim ipsum id lacus.\n Nullam rhoncus aliquam metus.\nLorem ipsum dolor sit amet, consectetuer adipiscing elit. Nam libero tempore, cum soluta\n nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat facere\n possimus, omnis voluptas assumenda est, omnis dolor repellendus. Fusce wisi.\n Etiam bibendum elit eget erat. Integer in sapien. Nulla pulvinar eleifend sem. Duis sapien\n nunc, commodo et, interdum suscipit, sollicitudin et, dolor. Integer in sapien. Duis bibendum,\n lectus ut viverra rhoncus, dolor nunc faucibus libero, eget facilisis enim ipsum id lacus.\n Nullam rhoncus aliquam metus.\nLorem ipsum dolor sit amet, consectetuer adipiscing elit. Nam libero tempore, cum soluta\n nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat facere\n possimus, omnis voluptas assumenda est, omnis dolor repellendus. Fusce wisi.\n Etiam bibendum elit eget erat. Integer in sapien. Nulla pulvinar eleifend sem. Duis sapien\n nunc, commodo et, interdum suscipit, sollicitudin et, dolor. Integer in sapien. Duis bibendum,\n lectus ut viverra rhoncus, dolor nunc faucibus libero, eget facilisis enim ipsum id lacus.\n Nullam rhoncus aliquam metus.\n");*/
            renderer.AddText(new Text() { Content="Hello world!",Position = new Vector2(0,0)});
            //renderer.AddText("Ahoj",new Vector2(0,0));

            program = new ShaderProgram(ShaderLoader.LoadVertexShaderFromResources("FontShader"),ShaderLoader.LoadFragmentShaderFromResources("FontShader"));
        }

        private void WindowInstance_Closed()
        {
            vertexArray.Dispose();
            renderer.Dispose();
        }
    }
}
