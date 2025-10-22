using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

public class Renderer
{
    private bool _disposed;

    private readonly Shader _shader;

    private readonly int _vertexArrayObject;
    private readonly int _vertexBufferObject;
    private readonly int _elementBufferObject;

    private List<float> _vertices;
    int _vertexCount;

    private readonly int[] _indices;

    public Renderer(Shader shader, int textureUnit = 0)
    {
        _shader = shader;

        _vertices = GenerateLineVertices(-10f, 10f, (float)Math.PI / 1000);
        _vertexCount = _vertices.Count;

        _vertexArrayObject = GL.GenVertexArray();
        _vertexBufferObject = GL.GenBuffer();
        _elementBufferObject = GL.GenBuffer();

        GL.BindVertexArray(_vertexArrayObject);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Count * sizeof(float), _vertices.ToArray(),
            BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false,
            3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
    }

    List<float> GenerateLineVertices(float start, float end, float step)
    {
        var verts = new List<float>();
        for (float x = start; x <= end; x += step)
        {
            verts.Add(x);
            verts.Add(0f);
            verts.Add(0f);
        }
        return verts;
    }


    public void DrawElements()
    {
        _shader.Use();

        GL.DrawArrays(PrimitiveType.Points, 0, _vertexCount / 3);
        GL.DrawArrays(PrimitiveType.LineStrip, 0, _vertexCount / 3);
    }

    public void Dispose()
    {
        if (_disposed) return;
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