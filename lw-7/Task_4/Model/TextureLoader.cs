using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace Task_4.Model;

public static class TextureLoader
{
    public static int Load(string file)
    {
        Bitmap bitmap = new(file);
        int textureID;
        GL.GenTextures(1, out textureID);
        GL.BindTexture(TextureTarget.Texture2D, textureID);

        BitmapData data = bitmap.LockBits(
            new Rectangle(0, 0, bitmap.Width, bitmap.Height),
            ImageLockMode.ReadOnly,
            System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
            data.Width, data.Height, 0, PixelFormat.Bgra,
            PixelType.UnsignedByte, data.Scan0);

        bitmap.UnlockBits(data);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        return textureID;
    }
    
    public static int CreateColoredTexture(int width, int height, byte r, byte g, byte b, byte a)
    {
        int textureID = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, textureID);

        // Массив данных RGBA: заполняем нужным цветом
        byte[] colorData = new byte[width * height * 4];
        for (int i = 0; i < colorData.Length; i += 4)
        {
            colorData[i + 0] = r;
            colorData[i + 1] = g;
            colorData[i + 2] = b;
            colorData[i + 3] = a;
        }

        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
            width, height, 0,
            PixelFormat.Rgba, PixelType.UnsignedByte, colorData);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        return textureID;
    }
}