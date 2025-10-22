using OpenTK.Graphics.OpenGL4;
using Task2.Shaders;

public class Renderer
{
    private bool _disposed;

    private readonly Shader _shader;

    public Renderer(Shader shader)
    {
        _shader = shader;
    }

    public void DrawElements(IReadOnlyList<TorusData> toruses, List<BufferData> data)
    {
        _shader.Use();

        _shader.SetInt("torusCount", toruses.Count);

        for (int i = 0; i < toruses.Count; i++)
        {
            _shader.SetVector3($"torusPositions[{i}]", toruses[i].Position);
            _shader.SetFloat($"torusMajorRadii[{i}]", toruses[i].R);
            _shader.SetFloat($"torusMinorRadii[{i}]", toruses[i].r);
        }

        for (int i = 0; i < toruses.Count; i++)
        {
            _shader.SetMatrix4("model", toruses[i].ModelMatrix);

            GL.BindVertexArray(data[i].VAO);
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, data[i].VertexCount);
        }

        GL.BindVertexArray(0);
    }

    public static BufferData CreateBufferData(float[] points)
    {
        int vao = GL.GenVertexArray();
        GL.BindVertexArray(vao);

        int vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, points.Length * sizeof(float), points, BufferUsageHint.StaticDraw);
        ConfigurateShaderLayout();

        GL.BindVertexArray(0);

        return new BufferData(vao, points.Length / 9);
    }

    private static void ConfigurateShaderLayout()
    {
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 9 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 9 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 9 * sizeof(float), 6 * sizeof(float));
        GL.EnableVertexAttribArray(2);
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