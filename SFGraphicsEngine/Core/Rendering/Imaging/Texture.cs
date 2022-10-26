using OpenTK.Graphics.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Runtime.InteropServices;

namespace SFGraphicsEngine.Core.Rendering.Imaging
{
    public class Texture
    {
        public virtual int Id { get; set; }

        public Texture(string path)
        {
            Image<Rgba32> img = Image.Load<Rgba32>(path);

            InitTexture(img);
        }

        public Texture(byte[] data,int width,int height)
        {
            Image<L8> img = Image.LoadPixelData<L8>(data,width,height);

            img.Mutate(x => x.Flip(FlipMode.Vertical));

            byte[] pd = new byte[width * height];
            img.CopyPixelDataTo(pd);

            Id = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Id);

            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.CompressedRed, width, height, 0, PixelFormat.Red, PixelType.UnsignedByte, pd);



            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
        public Texture(int id)
        {
            Id = id;
        }

        public Texture(IntPtr data,int width,int height)
        {
            byte[] dat = new byte[width * height];
            Marshal.Copy(data,dat,0, dat.Length);

            Image<L8> imgv = Image.LoadPixelData<L8>(dat,width,height);
            imgv.Mutate(x => x.Flip(FlipMode.Vertical));

            byte[] pd = new byte[width * height];
            imgv.CopyPixelDataTo(pd);

            Id = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Id);

            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.CompressedRed, width, height, 0, PixelFormat.Red, PixelType.UnsignedByte, pd);

            

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public Texture(Image<Rgba32> image) => InitTexture(image);

        private void InitTexture(Image<Rgba32> original)
        {
            original.Mutate(x => x.Flip(FlipMode.Vertical));

            byte[] data = new byte[original.Width * original.Height*4];
            original.CopyPixelDataTo(data);

            Id = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Id);

            GL.TexImage2D(TextureTarget.Texture2D,0,PixelInternalFormat.Rgba,original.Width,original.Height,0,PixelFormat.Rgba,PixelType.UnsignedByte,data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }


        public void Bind()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Id);
        }
    }
}
