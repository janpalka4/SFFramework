using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace SFGraphicsEngine.Core
{
    public class BufferObject<T> : IDisposable where T : struct
    {
        public int Id;
        public int Length { get; set; }

        private BufferTarget Target { get; set; }
        private T[] Data { get; set; }
        public BufferObject(T[] data, BufferTarget target = BufferTarget.ArrayBuffer, BufferUsageHint usageHint = BufferUsageHint.StaticDraw)
        {
            Target = target;
            Data = data;

            Length = data.Length;

            Id = GL.GenBuffer();

            GL.BindBuffer(target, Id);
            GL.BufferData(target, Marshal.SizeOf<T>() * data.Length, data, usageHint);
            GL.BindBuffer(target, 0);
        }

        public void Bind() => GL.BindBuffer(Target,Id);

        public void Unbind() => GL.BindBuffer(Target, 0);

        public void AppendData(T[] data)
        {
            List<T> dat = new List<T>();
            dat.AddRange(Data);
            dat.AddRange(data);          

            GL.BindBuffer(Target, Id);
            GL.BufferData(Target, Marshal.SizeOf<T>() * dat.Count, dat.ToArray(), BufferUsageHint.StaticDraw);
            GL.BindBuffer(Target,0);

            Data = dat.ToArray();
            Length = Data.Length;
        }

        public void Dispose()
        {
            GL.DeleteBuffer(Id);
        }
    }
}
