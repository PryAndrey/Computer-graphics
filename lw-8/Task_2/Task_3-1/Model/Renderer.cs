using MobiusStrip.Model;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Task2.Shaders;

public class Renderer
{
    private bool _disposed;

    private readonly Shader _shader;

    public Renderer(Shader shader)
    {
        _shader = shader;
    }

    public void DrawElements(IReadOnlyList<ParaboloidData> paraboloids, List<ParaboloidBufferData> data)
    {
        _shader.Use();

        _shader.SetInt("figureCount", paraboloids.Count);

        for (int i = 0; i < paraboloids.Count; i++)
        {
            _shader.SetVector3($"figurePositions[{i}]", paraboloids[i].Position);
            _shader.SetFloat($"paraboloidSizes[{i}]", paraboloids[i].Size);
            _shader.SetFloat($"paraboloidHeights[{i}]", paraboloids[i].MaxHeight);
        }

        foreach (ParaboloidBufferData bufferData in data)
        {
            GL.BindVertexArray(bufferData.VAO);
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, bufferData.VertexCount);

            GL.BindVertexArray(bufferData.CapVAO);
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, bufferData.CapVertexCount);

            GL.BindVertexArray(0);
        }
    }

    public static ParaboloidBufferData CreateBufferData(float[] paraboloidPoints, float[] capPoints)
    {
        int vao = GL.GenVertexArray();
        GL.BindVertexArray(vao);

        int vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, paraboloidPoints.Length * sizeof(float), paraboloidPoints, BufferUsageHint.StaticDraw);
        ConfigurateShaderLayout();

        int capVao = GL.GenVertexArray();
        GL.BindVertexArray(capVao);
        
        int capVbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, capVbo);
        GL.BufferData(BufferTarget.ArrayBuffer, capPoints.Length * sizeof(float), capPoints, BufferUsageHint.StaticDraw);
        ConfigurateShaderLayout();
        
        GL.BindVertexArray(0);

        return new ParaboloidBufferData(vao, paraboloidPoints.Length / 9, capVao, capPoints.Length / 9);
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

    public static float[] FillPoints(float[] points, Vector3 normal, Vector3 color)
    {
        List<float> data = [];

        for (int i = 0; i <= points.Length - 3; i += 3)
        {
            float x = points[i];
            float y = points[i + 1];
            float z = points[i + 2];

            data.AddRange([x, y, z, normal.X, normal.Y, normal.Z, color.X, color.Y, color.Z]);
        }

        return data.ToArray();
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