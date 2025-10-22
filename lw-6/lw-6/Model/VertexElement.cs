using OpenTK.Mathematics;
using Color4 = OpenTK.Mathematics.Color4;

public struct BufferData
{
    public int VAO;
    public int VertexCount;

    public BufferData(int vao, int vertexCount)
    {
        VAO = vao;
        VertexCount = vertexCount;
    }
}
