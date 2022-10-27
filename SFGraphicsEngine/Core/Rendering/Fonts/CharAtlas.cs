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
    /// <summary>
    /// Texture of font characters
    /// </summary>
    public class CharAtlas : Texture
    {
        const int chars = 128;
        public int FontSize { get; set; }
        public Texture atlasTexture { get; set; }
        public CharacterInfo[] CharacterInfos { get; set; }
        public override int Id { get; set; }

        /// <summary>
        /// Generates new char atlas from font
        /// </summary>
        /// <param name="font">Path to font (.ttf)</param>
        /// <param name="fontSize">Size of font to be generated</param>
        /// <exception cref="Exception"></exception>
        public CharAtlas(string font = "Res/Fonts/Lucida Sans Regular.ttf", int fontSize = 18) : base()
        {
            CharacterInfos = new CharacterInfo[chars];

            FontSize = fontSize;

            Character[] characters = loadAllCharacters(font);

            int w, h;

            byte[] data = GetPixelDataFromCharacters(characters, out w, out h);

            InitTexture(data,w,h,1,PixelInternalFormat.CompressedRed,PixelFormat.Red);

            LoadInfos(characters, w, h);
            
            
            
        }

        private void LoadInfos(Character[] characters, int w, int h)
        {
            int x = 0;
            for (int i = 0; i < characters.Length; i++)
            {
                CharacterInfos[i] = new CharacterInfo()
                {
                    AtlasPosition = new Vector2((float)x / (float)w, 0),
                    AtlasSize = new Vector2(characters[i].Size.X / (float)w, 1),
                    Bearing = characters[i].Bearing,
                    Size = characters[i].Size
                };
                x += characters[i].Size.X;
            }
        }

        private Character[] loadAllCharacters(string font)
        {
            FreeTypeLibrary library = new FreeTypeLibrary();

            IntPtr face;
            if (FT.FT_New_Face(library.Native, font, 0, out face) != FT_Error.FT_Err_Ok)
                throw new Exception("Cant init face");
            FreeTypeFaceFacade f = new FreeTypeFaceFacade(library, face);

            FT.FT_Set_Pixel_Sizes(face, 0, (uint)FontSize);


            Character[] characters = new Character[chars];
            int x = 0;
            for (int i = 0; i < characters.Length; i++)
            {
                characters[i] = loadChar((char)(i), face, f);

                x += characters[i].Size.X;
            }

            FT.FT_Done_Face(face);
            FT.FT_Done_FreeType(library.Native);

            return characters;
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

        private byte[] GetPixelDataFromCharacters(Character[] characters, out int width, out int height)
        {
            int w = characters.Sum(x => x.Size.X);
            int h = characters.Max(x => x.Size.Y);
            int x = 0;
            byte[] data = new byte[w * h];

            Image<L8> img = new Image<L8>(w, h);
            for (int i = 0; i < characters.Length; i++)
            {
                if (characters[i].Size.X > 0)
                {
                    Image<L8> img2 = Image<L8>.LoadPixelData<L8>(characters[i].data, characters[i].Size.X, characters[i].Size.Y);
                    Point p = new Point(x, h - characters[i].Size.Y);
                    img.Mutate(x => x.DrawImage(img2, p, 1f));
                    x += characters[i].Size.X;
                }
            }
            img.CopyPixelDataTo(data);

            width = w;
            height = h;

            return data;
        }
    }

    /// <summary>
    /// Character from font
    /// </summary>
    public struct Character
    {
        public Vector2i Size;
        public Vector2i Bearing;
        public byte[] data;
    }

    /// <summary>
    /// Info about character in atlas
    /// </summary>
    public struct CharacterInfo
    {
        public Vector2i Size;
        public Vector2i Bearing;
        public Vector2 AtlasPosition;
        public Vector2 AtlasSize;
    }
}
