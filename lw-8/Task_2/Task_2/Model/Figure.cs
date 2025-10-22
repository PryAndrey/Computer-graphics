using OpenTK.Mathematics;

public class Figure
{
    private static BufferData CreateFace(char axis, float position, Vector3 center,
        float halfSizeA, float halfSizeB, Vector3 color)
    {
        Vector3 normal = axis switch
        {
            'X' => new Vector3(position - center.X, 0, 0),
            'Y' => new Vector3(0, position - center.Y, 0),
            'Z' => new Vector3(0, 0, position - center.Z),
            _ => throw new ArgumentException("Invalid axis. Use 'X', 'Y' or 'Z'.", nameof(axis))
        };
        normal.Normalize();

        float[] points = axis switch
        {
            'X' => new[]
            {
                position, center.Y - halfSizeA, center.Z - halfSizeB,
                position, center.Y - halfSizeA, center.Z + halfSizeB,
                position, center.Y + halfSizeA, center.Z + halfSizeB,
                position, center.Y + halfSizeA, center.Z - halfSizeB
            },
            'Y' => new[]
            {
                center.X - halfSizeA, position, center.Z - halfSizeB,
                center.X - halfSizeA, position, center.Z + halfSizeB,
                center.X + halfSizeA, position, center.Z + halfSizeB,
                center.X + halfSizeA, position, center.Z - halfSizeB
            },
            'Z' => new[]
            {
                center.X - halfSizeA, center.Y - halfSizeB, position,
                center.X - halfSizeA, center.Y + halfSizeB, position,
                center.X + halfSizeA, center.Y + halfSizeB, position,
                center.X + halfSizeA, center.Y - halfSizeB, position
            },
            _ => throw new ArgumentException("Invalid axis. Use 'X', 'Y' or 'Z'.", nameof(axis))
        };

        return Renderer.CreateBufferData(Renderer.FillPoints(points, normal, color));
    }

    public static BufferData CreateSphere(Vector3 center, float radius, Vector3 color, int segments = 32)
    {
        List<float> points = new List<float>();

        for (int lat = 0; lat <= segments; lat++)
        {
            float theta = (float)lat * MathF.PI / segments;
            float sinTheta = MathF.Sin(theta);
            float cosTheta = MathF.Cos(theta);

            for (int lon = 0; lon <= segments; lon++)
            {
                float phi = (float)lon * 2f * MathF.PI / segments;
                float sinPhi = MathF.Sin(phi);
                float cosPhi = MathF.Cos(phi);

                float x = center.X + radius * sinTheta * cosPhi;
                float y = center.Y + radius * cosTheta;
                float z = center.Z + radius * sinTheta * sinPhi;

                Vector3 normal = new Vector3(sinTheta * cosPhi, cosTheta, sinTheta * sinPhi);
                normal.Normalize();

                points.Add(x);
                points.Add(y);
                points.Add(z);
                points.Add(normal.X);
                points.Add(normal.Y);
                points.Add(normal.Z);
                points.Add(color.X);
                points.Add(color.Y);
                points.Add(color.Z);
            }
        }

        List<uint> indices = new List<uint>();
        for (int lat = 0; lat < segments; lat++)
        {
            for (int lon = 0; lon < segments; lon++)
            {
                uint current = (uint)(lat * (segments + 1) + lon);
                uint next = (uint)(current + segments + 1);

                // Два треугольника образуют квад
                indices.Add(current);
                indices.Add(next);
                indices.Add(current + 1);

                indices.Add(next);
                indices.Add(next + 1);
                indices.Add(current + 1);
            }
        }

        float[] vertexData = new float[indices.Count * 9];
        for (int i = 0; i < indices.Count; i++)
        {
            int srcPos = (int)indices[i] * 9;
            Array.Copy(points.ToArray(), srcPos, vertexData, i * 9, 9);
        }

        return Renderer.CreateBufferData(vertexData);
    }

    public static List<BufferData> CreateCube(Vector3 center, float size, Vector3 color)
    {
        float halfSize = size / 2f;
        return new List<BufferData>
        {
            CreateFace('X', center.X - halfSize, center, halfSize, halfSize, color),
            CreateFace('X', center.X + halfSize, center, halfSize, halfSize, color),
            CreateFace('Y', center.Y - halfSize, center, halfSize, halfSize, color),
            CreateFace('Y', center.Y + halfSize, center, halfSize, halfSize, color),
            CreateFace('Z', center.Z - halfSize, center, halfSize, halfSize, color),
            CreateFace('Z', center.Z + halfSize, center, halfSize, halfSize, color)
        };
    }

    public static List<BufferData> CreateRectangle(Vector3 center, float sizeX, float sizeY, float sizeZ,
        Vector3 color)
    {
        return new List<BufferData>
        {
            CreateFace('X', center.X - sizeX / 2, center, sizeY / 2, sizeZ / 2, color),
            CreateFace('X', center.X + sizeX / 2, center, sizeY / 2, sizeZ / 2, color),
            CreateFace('Y', center.Y - sizeY / 2, center, sizeX / 2, sizeZ / 2, color),
            CreateFace('Y', center.Y + sizeY / 2, center, sizeX / 2, sizeZ / 2, color),
            CreateFace('Z', center.Z - sizeZ / 2, center, sizeX / 2, sizeY / 2, color),
            CreateFace('Z', center.Z + sizeZ / 2, center, sizeX / 2, sizeY / 2, color)
        };
    }

    public static BufferData[] CreateFloor()
    {
        Vector3 color = new(0.4f, 0.3f, 1f);

        Vector3 normalUp = new(0, 1, 0);
        Vector3 normalDown = new(0, -1, 0);
        var width = 3.0f;
        var length = 3.0f;

        float[] pointsUp =
        [
            -width, -1.2f, length,
            width, -1.2f, length,
            width, -1.2f, -length,
            -width, -1.2f, -length,
            -width, -1.2f, length,
        ];
        float[] pointsDown =
        [
            -width, -1.3f, -length,
            width, -1.3f, -length,
            width, -1.3f, length,
            -width, -1.3f, length,
            -width, -1.3f, -length,
        ];

        return
        [
            Renderer.CreateBufferData(Renderer.FillPoints(pointsUp, normalUp, color)),
            Renderer.CreateBufferData(Renderer.FillPoints(pointsDown, normalDown, color)),
        ];
    }
}