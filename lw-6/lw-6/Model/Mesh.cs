using Assimp;
using OpenTK.Graphics.OpenGL4;
using PrimitiveType = OpenTK.Graphics.OpenGL4.PrimitiveType;

public class Mesh
{
    public int VAO;
    public int VBO;
    public int EBO;
    public int IndexCount;
    public int MaterialIndex;

    public Mesh(float[] vertices, uint[] indices, int materialIndex)
    {
        MaterialIndex = materialIndex;
        IndexCount = indices.Length;

        VAO = GL.GenVertexArray();
        VBO = GL.GenBuffer();
        EBO = GL.GenBuffer();

        GL.BindVertexArray(VAO);

        GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

        int stride = 8 * sizeof(float); // пример: 3 pos + 3 normal + 2 texcoord

        // Позиция (location = 0)
        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);

        // Нормаль (location = 1)
        GL.EnableVertexAttribArray(1);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, stride, 3 * sizeof(float));

        // Текстурные координаты (location = 2)
        GL.EnableVertexAttribArray(2);
        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, stride, 6 * sizeof(float));

        GL.BindVertexArray(0);
    }

    public void Draw(MaterialLoader materialLoader, Material material)
    {
        materialLoader.ApplyMaterial(material, MaterialIndex);
        GL.BindVertexArray(VAO);
        GL.DrawElements(PrimitiveType.Triangles, IndexCount, DrawElementsType.UnsignedInt, 0);
        GL.BindVertexArray(0);
    }
}