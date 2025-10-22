using System.Drawing;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Tetris.Models;

namespace Tetris;

static class ToColor4Converter
{
    public static Color4 Convert(Color? color)
    {
        if (color == null)
            return Color4.Black;

        float r = color.Value.R / 255.0f;
        float g = color.Value.G / 255.0f;
        float b = color.Value.B / 255.0f;
        float a = color.Value.A / 255.0f;

        return new Color4(r, g, b, a);
    }
}

public class RGBVertex
{
    public const int ColorIndex = 3;
    public const int VertexSize = 6;

    public Vector3 Position;
    public Color4 Color;

    public RGBVertex(Vector3 position, Color4 color)
    {
        Position = position;
        Color = color;
    }

    public RGBVertex(Vector2 position, Color4 color)
    {
        Position = new Vector3(position.X, position.Y, 0f);
        Color = color;
    }

    public RGBVertex(float x, float y, Color4 color)
    {
        Position = new Vector3(x, y, 0f);
        Color = color;
    }

    public RGBVertex(float x, float y, float z, Color4 color)
    {
        Position = new Vector3(x, y, z);
        Color = color;
    }

    public static float[] ToFloatArray(RGBVertex vertex)
    {
        return
        [
            vertex.Position.X,
            vertex.Position.Y,
            vertex.Position.Z,
            vertex.Color.R,
            vertex.Color.G,
            vertex.Color.B
        ];
    }
}

public class TetrominoGenerator
{
    public static Tetromino Get()
    {
        Random random = new Random();
        int index = random.Next(Enum.GetValues(typeof(TetrominoType)).Length);

        TetrominoType type = (TetrominoType)index;

        Point position = new Point(GameBoard.Width / 2 - 1, 0);

        return new Tetromino(type, position);
    }
}

public class TextRenderer : IDisposable
{
    private Bitmap _bmp;
    private Graphics _gfx;
    private int _texture;
    private Rectangle _rectGFX;
    private bool _disposed;
    private PointF _position;

    public Font font = new Font(FontFamily.GenericSansSerif, 24);

    // Конструктор нового экземпляра класса
    public TextRenderer(int width, int height)
    {
        _bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        _gfx = Graphics.FromImage(_bmp);
        // Используем сглаживание
        _gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

        // Генерация текстуры
        _texture = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, _texture);

        // Свойства текстуры
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        // Создание пустой текстуры
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
            width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);

        _position = PointF.Empty;
    }

    // Заливка образа цветом color
    public void Clear(Color color)
    {
        _position = PointF.Empty;
        _gfx.Clear(color);
        _rectGFX = new Rectangle(0, 0, _bmp.Width, _bmp.Height);
    }

    public void DrawNewString(string text, Brush brush)
    {
        _position.Y += font.Height;
        _gfx.DrawString(text, font, brush, _position);
    }

    // Получает обработчик текстуры (System.Int32), который связывается с TextureTarget.Texture2D
    public int Texture
    {
        get
        {
            UploadBitmap(); // Загружаем растровые данные в текстуру
            return _texture;
        }
    }

    public void Use()
    {
        GL.ActiveTexture((TextureUnit)33984); // Активируем нужный текстурный юнит
        GL.BindTexture(TextureTarget.Texture2D, Texture); // Привязываем текстуру
    }

    // Выгружает растровые данные в текстуру OpenGL
    private void UploadBitmap()
    {
        if (_rectGFX != RectangleF.Empty)
        {
            System.Drawing.Imaging.BitmapData data = _bmp.LockBits(_rectGFX,
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.BindTexture(TextureTarget.Texture2D, _texture);

            // Текстура формируется на основе растровых данных
            GL.TexSubImage2D(TextureTarget.Texture2D, 0,
                _rectGFX.X, _rectGFX.Y, _rectGFX.Width, _rectGFX.Height,
                PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            // Освобождаем память, занимаемую data
            _bmp.UnlockBits(data);
            _rectGFX = Rectangle.Empty;
        }
    }

    private void Dispose(bool manual)
    {
        if (!_disposed)
        {
            if (manual)
            {
                _bmp.Dispose();
                _gfx.Dispose();

                // Освобождение текстуры
                GL.DeleteTexture(_texture);
            }

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}