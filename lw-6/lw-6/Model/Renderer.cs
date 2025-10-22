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

    public void DrawElements(PrimitiveType primitiveType,
        List<Vector3> centres,
        List<float> sizes,
        List<BufferData> datas)
    {
        _shader.Use();

        _shader.SetInt("figureCount", centres.Count());

        Vector3[] centresMas = centres.ToArray();
        float[] sizesMas = sizes.ToArray();
        for (int i = 0; i < centresMas.Count(); i++)
        {
            _shader.SetVector3($"figureCentres[{i}]", centres[i]);
            _shader.SetFloat($"figureSizes[{i}]", sizesMas[i]);
        }

        foreach (BufferData bufferData in datas)
        {
            GL.BindVertexArray(bufferData.VAO);

            GL.DrawArrays(primitiveType, 0, bufferData.VertexCount);
        }
    }

    public static BufferData CreateBufferData(float[] points)
    {
        // float[] reversed = ReverseByChunks(points);
        int vao = GL.GenVertexArray();
        GL.BindVertexArray(vao);

        int vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, points.Length * sizeof(float), points, BufferUsageHint.StaticDraw);
        ConfigurateShaderLayout();

        GL.BindVertexArray(0);

        return new BufferData(vao, points.Length / 9);
    }

    public static float[] ReverseByChunks(float[] points, int chunkSize = 9)
    {
        if (points == null || points.Length % chunkSize != 0)
            throw new ArgumentException("Invalid array length for chunk size");

        float[] reversed = new float[points.Length];
        int chunksCount = points.Length / chunkSize;

        for (int i = 0; i < chunksCount; i++)
        {
            int sourceIndex = (chunksCount - 1 - i) * chunkSize;
            int targetIndex = i * chunkSize;

            Array.Copy(points, sourceIndex, reversed, targetIndex, chunkSize);
        }

        return reversed;
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
        List<float> fillPoints = [];

        for (int i = 0; i <= points.Length - 3; i += 3)
        {
            float x = points[i];
            float y = points[i + 1];
            float z = points[i + 2];

            fillPoints.AddRange([x, y, z, normal.X, normal.Y, normal.Z, color.X, color.Y, color.Z]);
        }

        return fillPoints.ToArray();
    }

    public static float[] FillTriangles(float[] points, float[] normal, Vector3 color)
    {
        List<float> fillPoints = [];

        for (int i = 0; i <= points.Length - 3; i += 3)
        {
            float x = points[i];
            float y = points[i + 1];
            float z = points[i + 2];

            fillPoints.AddRange([x, y, z, normal[i], normal[i + 1], normal[i + 2], color.X, color.Y, color.Z]);
        }

        return fillPoints.ToArray();
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