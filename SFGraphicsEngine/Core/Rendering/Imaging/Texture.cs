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

            InitFromImage(img);
        }

        public Texture(byte[] data, int width, int height) => InitTexture(data, width, height);
        
        public Texture()
        {
        }

        public Texture(Image<Rgba32> image) => InitFromImage(image);

        protected void InitFromImage(Image<Rgba32> image)
        {
            byte[] data = new byte[image.Width * image.Height];
            image.CopyPixelDataTo(data);

            InitTexture(data, image.Width, image.Height);
        }

        protected void InitTexture(byte[] data,int width,int height, int? unpackAligment = null,  PixelInternalFormat format = PixelInternalFormat.Rgba, PixelFormat pixelFormat = PixelFormat.Rgba)
        {
            Id = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Id);

            if(unpackAligment.HasValue)
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, unpackAligment.Value);

            GL.TexImage2D(TextureTarget.Texture2D,0,format,width,height,0,pixelFormat,PixelType.UnsignedByte,data);

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
