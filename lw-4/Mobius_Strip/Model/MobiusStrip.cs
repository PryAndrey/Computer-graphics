using MobiusStrip.Utilities;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace MobiusStrip;

public class MobiusStrip
{
    private const float MinU = 0f;
    private const float MaxU = MathHelper.TwoPi;

    private const float MinV = -1f;
    private const float MaxV = 1f;

    private const int SegmentsU = 60;

    private List<VertexElement> _verticesList;

    public MobiusStrip()
    {
        InitializeVertices();
    }

    public void Draw(Renderer renderer, Vector3 position)
    {
        CalculateNormals();
        renderer.DrawElements(PrimitiveType.TriangleStrip, _verticesList, position);
        // renderer.DrawElements(PrimitiveType.LineLoop, _verticesList, position);
        // todo Выставить z функцив в <= для линии
    }

    private void InitializeVertices()
    {
        _verticesList = [];

        var u0 = MathHelper.Lerp(MinU, MaxU, 0f);
        _verticesList.Add(GetVertexElement(u0, MinV));
        _verticesList.Add(GetVertexElement(u0, MaxV));

        for (int i = 1; i < SegmentsU; i++)
        {
            var u = MathHelper.Lerp(MinU, MaxU, (float)i / (SegmentsU - 1));
            _verticesList.Add(GetVertexElement(u, MinV));
            _verticesList.Add(GetVertexElement(u, MaxV));
        }

        CalculateNormals();
    }

    private static VertexElement GetVertexElement(float u, float v)
    {
        var position = new Vector3(GetX(u, v), GetY(u, v), GetZ(u, v));
        var color = new Color4(position.X * 3, position.Y * 3, position.Z * 3, 255);

        return new VertexElement(position, color);
    }

    private static float GetX(float u, float v)
    {
        return (float)((1 + v / 2 * MathHelper.Cos(u / 2)) * MathHelper.Cos(u));
    }

    private static float GetY(float u, float v)
    {
        return (float)((1 + v / 2 * MathHelper.Cos(u / 2)) * MathHelper.Sin(u));
    }

    private static float GetZ(float u, float v)
    {
        return (float)(v / 2 * MathHelper.Sin(u / 2));
    }

    public void CalculateNormals()
    {
        for (int i = 0; i < _verticesList.Count; i++)
        {
            var v = _verticesList[i];
            v.Normal = Vector3.Zero;
            _verticesList[i] = v;
            if (i < 2)
                continue;

            Vector3 v1 = _verticesList[i - 2].Position;
            Vector3 v2 = _verticesList[i - 1].Position;
            Vector3 v3 = _verticesList[i].Position;

            Vector3 normal = Vector3.Cross(v2 - v1, v3 - v1).Normalized();

            AddNormalToVertex(i - 2, normal);
            AddNormalToVertex(i - 1, normal);
            AddNormalToVertex(i, normal);
        }

        for (int i = 0; i < _verticesList.Count; i++)
        {
            var v = _verticesList[i];
            v.Normal = v.Normal.Normalized();
            _verticesList[i] = v;
        }
    }

    private void AddNormalToVertex(int index, Vector3 normal)
    {
        var vertex = _verticesList[index];
        vertex.Normal += normal;
        _verticesList[index] = vertex;
    }
}