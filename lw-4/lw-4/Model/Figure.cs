using Cuboctahedron.Utilities;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Cuboctahedron;

public class Figure
{
    private static readonly Color4 EdgeColor = Color4.Black;
    private static readonly Color4 TriangleColor = Color4.Khaki;
    private static readonly Color4 SquareColor = Color4.SeaGreen;

    private readonly Vector3[] _vertices =
    {
        new Vector3(-1, 1, 0), 
        new Vector3(0, 1, 1), 
        new Vector3(1, 1, 0), 
        new Vector3(0, 1, -1), 
        new Vector3(-1, -1, 0), 
        new Vector3(0, -1, 1), 
        new Vector3(1, -1, 0), 
        new Vector3(0, -1, -1),
        new Vector3(-1, 0, 1), 
        new Vector3(1, 0, 1), 
        new Vector3(1, 0, -1), 
        new Vector3(-1, 0, -1) 
    };

    private static readonly int[] EdgeIndices =
    [
        0, 1, 1, 2, 2, 3, 0, 3,
        4, 5, 5, 6, 6, 7, 7, 4,
        0, 11, 11, 4, 4, 8, 8, 0,
        1, 8, 8, 5, 5, 9, 9, 1,
        2, 9, 9, 6, 6, 10, 10, 2,
        3, 10, 10, 7, 7, 11, 11, 3
    ];

    private static readonly int[] SquareIndices =
    [
        0, 1, 2, 3,
        7, 6, 5, 4,
        0, 11, 4, 8,
        1, 8, 5, 9,
        2, 9, 6, 10,
        3, 10, 7, 11
    ];

    private static readonly int[] TriangleIndices =
    [
        3, 11, 0,
        4, 11, 7,
        0, 8, 1,
        5, 8, 4,
        1, 9, 2,
        6, 9, 5,
        2, 10, 3,
        7, 10, 6
    ];

    private readonly List<VertexElement> _rgbVerticesList;

    public Figure()
    {
        _rgbVerticesList = _vertices.Select(v => new VertexElement(v)).ToList();
        CalculateNormals();
    }

    public void Draw(Renderer renderer, Vector3 position)
    {
        SetVerticesColor(EdgeColor);
        renderer.DrawElements(PrimitiveType.Lines, _rgbVerticesList, EdgeIndices, position, 2);

        SetVerticesColor(TriangleColor);
        renderer.DrawElements(PrimitiveType.Triangles, _rgbVerticesList, TriangleIndices, position);
        
        SetVerticesColor(SquareColor);
        renderer.DrawElements(PrimitiveType.Quads, _rgbVerticesList, SquareIndices, position);
    }

    private void SetVerticesColor(Color4 color)
    {
        for (int i = 0; i < _rgbVerticesList.Count; i++)
        {
            var vertex = _rgbVerticesList[i];
            vertex.Color = color;
            _rgbVerticesList[i] = vertex;
        }
    }

    public void CalculateNormals()
    {
        for (int i = 0; i < _rgbVerticesList.Count; i++)
        {
            var v = _rgbVerticesList[i];
            v.Normal = Vector3.Zero;
            _rgbVerticesList[i] = v;
        }
        
        for (int i = 0; i < TriangleIndices.Length; i += 3)
        {
            int i1 = TriangleIndices[i];
            int i2 = TriangleIndices[i + 1];
            int i3 = TriangleIndices[i + 2];

            Vector3 v1 = _rgbVerticesList[i1].Position;
            Vector3 v2 = _rgbVerticesList[i2].Position;
            Vector3 v3 = _rgbVerticesList[i3].Position;

            Vector3 normal = Vector3.Cross(v2 - v1, v3 - v1).Normalized();
            
            AddNormalToVertex(i1, normal);
            AddNormalToVertex(i2, normal);
            AddNormalToVertex(i3, normal);
        }
        
        for (int i = 0; i < SquareIndices.Length; i += 4)
        {
            int i1 = SquareIndices[i];
            int i2 = SquareIndices[i + 1];
            int i3 = SquareIndices[i + 2];
            int i4 = SquareIndices[i + 3];

            Vector3 v1 = _rgbVerticesList[i1].Position;
            Vector3 v2 = _rgbVerticesList[i2].Position;
            Vector3 v3 = _rgbVerticesList[i3].Position;
            Vector3 v4 = _rgbVerticesList[i4].Position;

            
            Vector3 normal1 = Vector3.Cross(v2 - v1, v3 - v1).Normalized();

            
            Vector3 normal2 = Vector3.Cross(v3 - v1, v4 - v1).Normalized();

            
            Vector3 avgNormal = (normal1 + normal2).Normalized();

            
            AddNormalToVertex(i1, avgNormal);
            AddNormalToVertex(i2, avgNormal);
            AddNormalToVertex(i3, avgNormal);
            AddNormalToVertex(i4, avgNormal);
        }

        
        for (int i = 0; i < _rgbVerticesList.Count; i++)
        {
            var v = _rgbVerticesList[i];
            v.Normal = v.Normal.Normalized();
            _rgbVerticesList[i] = v;
        }
    }

    private void AddNormalToVertex(int index, Vector3 normal)
    {
        var vertex = _rgbVerticesList[index];
        vertex.Normal += normal;
        _rgbVerticesList[index] = vertex;
    }// Подходит для криволинейной поверхности
}    // todo Можно дублировать вершины