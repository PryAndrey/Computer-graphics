using OpenTK.Mathematics;

public struct ParaboloidData
{
    public float Size;
    public float MaxHeight;

    public Vector3 Position;

    public ParaboloidData(
        float size,
        float maxHeight,
        Vector3 position)
    {
        MaxHeight = maxHeight;
        Position = position;
        Size = size;
    }
}

public class Figure
{
    public static float[] GetParaboloidPoints(ParaboloidData data, Vector3 color, int segments, int depthStacks)
    {
        List<float> points = [];

        for (int i = 0; i < segments; i++)
        {
            float u0 = (float)i / segments * MathF.PI * 2;
            float u1 = (float)(i + 1) / segments * MathF.PI * 2;

            for (int j = 0; j <= depthStacks; j++)
            {
                float v = (float)j / depthStacks;
                AddVertex(points, data.Size, data.Position, data.MaxHeight, u0, v, color);
                AddVertex(points, data.Size, data.Position, data.MaxHeight, u1, v, color);
            }
        }

        return points.ToArray();
    }

    private static void AddVertex(List<float> data, float size, Vector3 position,
        float height, float u, float v, Vector3 color)
    {
        float x = size * v * MathF.Cos(u);
        float y = size * v * MathF.Sin(u);
        float z = height * v * v;

        Vector3 pos = new Vector3(x, z, y) + position;

        Vector3 normal = Vector3.Normalize(new Vector3(2 * x / (size * size), 2 * y / (size * size), -1));

        data.AddRange([
            pos.X, pos.Y, pos.Z,
            normal.X, normal.Z, normal.Y,
            color.X, color.Y, color.Z,
        ]);
    }

    public static float[] GetParaboloidCap(ParaboloidData data, Vector3 color, int segments)
    {
        List<float> points =
        [
            data.Position.X, data.Position.Y + data.MaxHeight, data.Position.Z
        ];

        for (int i = 0; i <= segments; i++)
        {
            float u = (float)i / segments * MathF.PI * 2;
            float x = data.Size * MathF.Cos(u);
            float y = data.Size * MathF.Sin(u);

            points.AddRange([
                x + data.Position.X,
                data.MaxHeight + data.Position.Y,
                y + data.Position.Z,
            ]);
        }

        return Renderer.FillPoints(points.ToArray(), new(0, 1, 0), color);
    }
}