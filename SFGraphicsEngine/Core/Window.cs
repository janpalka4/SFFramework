using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using SFGraphicsEngine.Core.Rendering;
using SFGraphicsEngine.Core.Rendering.Shading;

namespace SFGraphicsEngine.Core
{
    public class Window
    {
        protected GameWindow WindowInstance;
        public Window(int Width = 800, int Height = 600, string Title = "GUIWindow")
        {
            NativeWindowSettings nativeSettings = new NativeWindowSettings()
            {
                API = ContextAPI.OpenGL,
                APIVersion = Version.Parse("3.3"),
                Profile = ContextProfile.Compatability
            };

            WindowInstance = new GameWindow(GameWindowSettings.Default, nativeSettings)
            {
                Size = new Vector2i(Width, Height),
                Title = Title,
                VSync = VSyncMode.On  
            };

            WindowInstance.Resize += WindowInstance_Resize;
            WindowInstance.Load += WindowInstance_Load;
            WindowInstance.RenderFrame += WindowInstance_RenderFrame;

            WindowInstance.Run();
        }

        protected virtual void WindowInstance_RenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            WindowInstance.SwapBuffers();
        }

        protected virtual void WindowInstance_Load()
        {
            GL.ClearColor(Color4.Black);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.DepthTest);
        }

        protected virtual void WindowInstance_Resize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);

            GlobalUniforms.Projection = Matrix4.CreateOrthographicOffCenter(0,e.Width, e.Height,0, -.1f, 100f);
            //GlobalUniforms.View = Matrix4.LookAt(new Vector3(400,300,1), new Vector3(400,300, 0), Vector3.UnitY);
        }
    }
}
