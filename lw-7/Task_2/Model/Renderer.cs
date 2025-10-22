using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

public class Renderer
{
    private bool _disposed;

    private readonly Shader _shader;

    private readonly int _vertexArrayObject;
    private readonly int _vertexBufferObject;

    private List<float> _vertices;
    int _vertexCount;

    private readonly int[] _indices;

    public Renderer(Shader shader, int textureUnit = 0)
    {
        _shader = shader;

        _vertices = GenerateVertices();
        _vertexCount = _vertices.Count;

        _vertexArrayObject = GL.GenVertexArray();
        _vertexBufferObject = GL.GenBuffer();

        GL.BindVertexArray(_vertexArrayObject);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Count * sizeof(float), _vertices.ToArray(),
            BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false,
            3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
    }

    List<float> GenerateVertices()
    {
        var vertices = new List<float>
        {
            -1.0f, -1.0f, 0.0f,
            1.0f, -1.0f, 0.0f,
            1.0f, 1.0f, 0.0f,
            -1.0f, 1.0f, 0.0f
        };

        return vertices;
    }

    public void DrawElements()
    {
        _shader.Use();
        GL.DrawArrays(PrimitiveType.TriangleFan, 0, _vertexCount / 3);
    }

    public void Dispose()
    {
        if (_disposed) return;
        GL.DeleteVertexArray(_vertexArrayObject);
        GL.DeleteBuffer(_vertexBufferObject);

        _disposed = true;
        GC.SuppressFinalize(this);
    }

    ~Renderer()
    {
        Dispose();
    }
}