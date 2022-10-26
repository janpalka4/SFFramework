using FreeTypeSharp;
using FreeTypeSharp.Native;
using System.Runtime.InteropServices;
using OpenTK.Mathematics;
using SFGraphicsEngine.Core.Rendering.Imaging;
using OpenTK.Graphics.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace SFGraphicsEngine.Core.Rendering.Fonts
{
    public class CharAtlas : Texture
    {
        const int chars = 128;
        public int FontSize { get; set; }
        public Texture atlasTexture { get; set; }
        public CharacterInfo[] CharacterInfos { get; set; }
        public override int Id { get; set; }

        private byte[] data { get; set; }

        public CharAtlas(string font, int fontSize) : base(0)
        {
            FontSize = fontSize;

            FreeTypeLibrary library = new FreeTypeLibrary();

            IntPtr face;
            if (FT.FT_New_Face(library.Native, "Res/Fonts/Lucida Sans Regular.ttf", 0, out face) != FT_Error.FT_Err_Ok)
                throw new Exception("Cant init face");
            FreeTypeFaceFacade f = new FreeTypeFaceFacade(library, face);

            FT.FT_Set_Pixel_Sizes(face, 0, (uint)fontSize);

            
            Character[] characters = new Character[chars];
            int x = 0;
            for (int i = 0; i < characters.Length; i++)
            {
                characters[i] = loadChar((char)(i), face, f);
                
                x += characters[i].Size.X;
            }
            int w = characters.Sum(x => x.Size.X);
            int h = characters.Max(x => x.Size.Y);
            x = 0;
            data = new byte[w*h];
            Image<L8> img = new Image<L8>(w,h);
            for (int i = 0; i < characters.Length; i++) {
                if (characters[i].Size.X > 0)
                {
                    Image<L8> img2 = Image<L8>.LoadPixelData<L8>(characters[i].data, characters[i].Size.X, characters[i].Size.Y);
                    Point p = new Point(x, h - characters[i].Size.Y);
                    img.Mutate(x => x.DrawImage(img2, p, 1f));
                    x += characters[i].Size.X;
                }
            }
            img.CopyPixelDataTo(data);
            //img.SaveAsBmp(@"C:\Users\janpa\atlas.bmp");
            InitTexture(characters);

            FT.FT_Done_Face(face);
            FT.FT_Done_FreeType(library.Native);
            
        }

        private Character loadChar(char ch, IntPtr face, FreeTypeFaceFacade facade)
        {
            FT.FT_Load_Char(face, ch, FT.FT_LOAD_RENDER);

            byte[] ret = new byte[facade.GlyphBitmap.width * facade.GlyphBitmap.rows];
            if (ret.Length > 0)
                Marshal.Copy(facade.GlyphBitmap.buffer, ret, 0, ret.Length);

            return new Character()
            {
                Size = new Vector2i((int)facade.GlyphBitmap.width, (int)facade.GlyphBitmap.rows),
                Bearing = new Vector2i(facade.GlyphBitmapLeft, facade.GlyphBitmapTop),
                data = ret
            };
        }

        private void InitTexture(Character[] chars)
        {
            CharacterInfos = new CharacterInfo[chars.Length];

            Id = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Id);

            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

            int width = chars.Sum(x => x.Size.X);
            int height = chars.Max(x => x.Size.Y);
            byte[] pixels = new byte[width * height];
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.CompressedRed, width, height, 0, PixelFormat.Red, PixelType.UnsignedByte, data);
            int x = 0;
            for (int i = 0; i < chars.Length; i++)
            {
                CharacterInfos[i] = new CharacterInfo()
                {
                    AtlasPosition = new Vector2((float)x / (float)width,0),
                    AtlasSize = new Vector2(chars[i].Size.X / (float)width, 1),
                    Bearing = chars[i].Bearing,
                    Size = chars[i].Size
                };
                x += chars[i].Size.X;
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
    }


    public struct Character
    {
        public Vector2i Size;
        public Vector2i Bearing;
        public byte[] data;
    }
    public struct CharacterInfo
    {
        public Vector2i Size;
        public Vector2i Bearing;
        public Vector2 AtlasPosition;
        public Vector2 AtlasSize;
    }
}
