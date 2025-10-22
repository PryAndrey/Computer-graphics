using OpenTK.Mathematics;

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