using OpenTK.Mathematics;

public struct TorusData
{
    public Vector3 Position;
    public float r;
    public float R;
    public Matrix4 ModelMatrix;

    public TorusData(Vector3 position, float Rt, float rt)
    {
        Position = position;
        R = Rt;
        r = rt;
        ModelMatrix = Matrix4.CreateTranslation(position);
    }
}

public class Figure
{
    public static float[] CreateTorusPoints(TorusData torus, Vector3 color, int segments, int crossSectionSegments)
    {
        List<float> data = [];

        for (int i = 0; i < segments; i++)
        {
            float u0 = (float)i / segments * 2 * MathF.PI;
            float u1 = (float)(i + 1) / segments * 2 * MathF.PI;

            for (int j = 0; j <= crossSectionSegments; j++)
            {
                float v = (float)j / crossSectionSegments * 2 * MathF.PI;

                data.AddRange(AddTorusVertex(torus, u0, v, color));
                data.AddRange(AddTorusVertex(torus, u1, v, color));
            }
        }

        return data.ToArray();
    }

    private static float[] AddTorusVertex(TorusData torus, float u, float v, Vector3 color)
    {
        float R = torus.R;
        float r = torus.r;

        float cosU = MathF.Cos(u);
        float sinU = MathF.Sin(u);
        float cosV = MathF.Cos(v);
        float sinV = MathF.Sin(v);

        float x = (R + r * cosV) * cosU;
        float y = (R + r * cosV) * sinU;
        float z = r * sinV;

        Vector3 pos = new(x, y, z);

        Vector3 normal = new(cosV * cosU, cosV * sinU, sinV);
        normal = Vector3.Normalize(normal);

        return
        [
            pos.X, pos.Z, pos.Y,
            normal.X, normal.Z, normal.Y,
            color.X, color.Y, color.Z
        ];
    }
}