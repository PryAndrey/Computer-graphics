using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using StbImageSharp;

public class Renderer
{
    private bool _disposed;

    private readonly Shader _shader;

    private readonly int _vertexArrayObject;
    private readonly int _vertexBufferObject;
    private readonly int _elementBufferObject;

    private readonly Dictionary<float, int> _textures = new Dictionary<float, int>();
    private readonly int _textureUnit;

    public Renderer(Shader shader, int textureUnit = 0)
    {
        _shader = shader;
        _textureUnit = textureUnit;

        _vertexArrayObject = GL.GenVertexArray();
        _vertexBufferObject = GL.GenBuffer();
        _elementBufferObject = GL.GenBuffer();

        GL.BindVertexArray(_vertexArrayObject);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, 0, IntPtr.Zero, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, 0, IntPtr.Zero, BufferUsageHint.DynamicDraw);

        var positionLocation = _shader.GetAttribLocation("aPosition");
        GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false,
            VertexElement.Size * sizeof(float), 0);
        GL.EnableVertexAttribArray(positionLocation);

        var colorLocation = _shader.GetAttribLocation("aColor");
        GL.VertexAttribPointer(colorLocation, 3, VertexAttribPointerType.Float, false,
            VertexElement.Size * sizeof(float), VertexElement.ColorIndex * sizeof(float));
        GL.EnableVertexAttribArray(colorLocation);

        var normalLocation = _shader.GetAttribLocation("aNormal");
        GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false,
            VertexElement.Size * sizeof(float), VertexElement.NormalIndex * sizeof(float));
        GL.EnableVertexAttribArray(normalLocation);

        var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
        GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false,
            VertexElement.Size * sizeof(float), VertexElement.TexCoordIndex * sizeof(float));
        GL.EnableVertexAttribArray(texCoordLocation);

        LoadTextures();
    }
    
    private void LoadTextures()
    {
        
        for (int i = 1; i <= 8; i++)
        {
            string texturePath = $"../../../Model/Textures/{i}.jpg";
            _textures[i] = LoadTexture(texturePath);
        }
        
        if (!_textures.ContainsKey(9))
        {
            string texturePath = $"../../../Model/Textures/Sky.jpg";
            
            _textures[9] = LoadTexture(texturePath);
        }
        
        if (!_textures.ContainsKey(10))
        {
            string texturePath = $"../../../Model/Textures/Sky.jpg";
            
            _textures[9] = LoadTexture(texturePath);
        }
        
        if (!_textures.ContainsKey(0))
        {
            _textures[0] = CreateSolidColorTexture(255, 255, 255); 
        }
    }

    private int LoadTexture(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Texture file not found: {path}");
        }

        
        int textureId = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, textureId);

        
        using (var stream = File.OpenRead(path))
        {
            var image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
        
            
            GL.TexImage2D(TextureTarget.Texture2D, 
                0, 
                PixelInternalFormat.Rgba, 
                image.Width, 
                image.Height, 
                0, 
                PixelFormat.Rgba, 
                PixelType.UnsignedByte, 
                image.Data);
        }

        
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
    
        
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

        
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

        return textureId;
    }
    
    private int CreateSolidColorTexture(byte r, byte g, byte b, byte a = 255)
    {
        int textureId = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, textureId);
    
        
        byte[] color = { r, g, b, a };
        GL.TexImage2D(TextureTarget.Texture2D, 
            0, 
            PixelInternalFormat.Rgba, 
            1, 1, 
            0, 
            PixelFormat.Rgba, 
            PixelType.UnsignedByte, 
            color);
    
        
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
    
        return textureId;
    }
    
    public void DrawElements(PrimitiveType primitiveType,
        float[] vertices,
        int[] indices,
        Vector3 modelMatrixPosition,
        float blockType,
        int thickness = 1)
    {
        _shader.Use();

        var model = Matrix4.CreateTranslation(modelMatrixPosition);
        _shader.SetMatrix4("model", model);

        if (_textures.TryGetValue(blockType, out int textureId))
        {
            GL.ActiveTexture(TextureUnit.Texture0 + _textureUnit);
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            _shader.SetInt("textureSampler", _textureUnit);
        }
        
        // GL.ActiveTexture(TextureUnit.Texture0 + _textureUnit);
        // GL.BindTexture(TextureTarget.Texture2D, 10);
        // _shader.SetInt("textureShade", _textureUnit);
        
        UpdateBuffers(vertices, indices);

        GL.LineWidth(thickness);

        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawElements(primitiveType, indices.Length, DrawElementsType.UnsignedInt, 0);
    }

    private void UpdateBuffers(float[] vertices, int[] indices)
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.DynamicDraw);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices,
            BufferUsageHint.DynamicDraw);
    }

    public void Dispose()
    {
        if (_disposed) return;
        foreach (var texture in _textures.Values)
        {
            GL.DeleteTexture(texture);
        }
        GL.DeleteVertexArray(_vertexArrayObject);
        GL.DeleteBuffer(_vertexBufferObject);
        GL.DeleteBuffer(_elementBufferObject);

        _disposed = true;
        GC.SuppressFinalize(this);
    }

    ~Renderer()
    {
        Dispose();
    }
}