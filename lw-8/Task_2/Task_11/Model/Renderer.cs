using MobiusStrip.Model;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Task2.Shaders;

public class Renderer
{
    private bool _disposed;

    private int _shaderProgram;
    private int _vertexArrayObject;

    private readonly Shader _shader;

    public Renderer(Shader shader)
    {
        _shader = shader;

        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);

        int vertexBuffer = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, MyScene.quadVertices.Length * sizeof(float), MyScene.quadVertices,
            BufferUsageHint.StaticDraw);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
        GL.EnableVertexAttribArray(0);
    }

    public void DrawElements()
    {
        _shader.Use();

        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
    }

    public void Dispose()
    {
        if (_disposed) return;

        _disposed = true;
        GC.SuppressFinalize(this);
    }

    ~Renderer()
    {
        Dispose();
    }
}