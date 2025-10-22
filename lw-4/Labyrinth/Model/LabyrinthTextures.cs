using OpenTK.Mathematics;

public class LabyrinthTextures
{
    public static List<VertexElement> GetVerticesList(float height, float size, Color4 color)
    {
        return new List<VertexElement>()
        {
            // Передняя грань
            new(new Vector3(0f, 0f, 0f), color, -Vector3.UnitZ, new Vector2(0, 0)),
            new(new Vector3(0f, height, 0f), color, -Vector3.UnitZ, new Vector2(0, 1)),
            new(new Vector3(size, height, 0f), color, -Vector3.UnitZ, new Vector2(1, 1)),
            new(new Vector3(size, 0f, 0f), color, -Vector3.UnitZ, new Vector2(1, 0)),

            // Задняя грань
            new(new Vector3(0f, 0f, size), color, Vector3.UnitZ, new Vector2(0, 0)),
            new(new Vector3(size, 0f, size), color, Vector3.UnitZ, new Vector2(1, 0)),
            new(new Vector3(size, height, size), color, Vector3.UnitZ, new Vector2(1, 1)),
            new(new Vector3(0f, height, size), color, Vector3.UnitZ, new Vector2(0, 1)),

            // Левая грань
            new(new Vector3(0f, 0f, 0f), color, -Vector3.UnitX, new Vector2(0, 0)),
            new(new Vector3(0f, 0f, size), color, -Vector3.UnitX, new Vector2(1, 0)),
            new(new Vector3(0f, height, size), color, -Vector3.UnitX, new Vector2(1, 1)),
            new(new Vector3(0f, height, 0f), color, -Vector3.UnitX, new Vector2(0, 1)),

            // Правая грань
            new(new Vector3(size, 0f, 0f), color, Vector3.UnitX, new Vector2(0, 0)),
            new(new Vector3(size, height, 0f), color, Vector3.UnitX, new Vector2(0, 1)),
            new(new Vector3(size, height, size), color, Vector3.UnitX, new Vector2(1, 1)),
            new(new Vector3(size, 0f, size), color, Vector3.UnitX, new Vector2(1, 0)),

            // Нижняя грань
            new(new Vector3(0f, 0f, 0f), color, -Vector3.UnitY, new Vector2(0, 0)),
            new(new Vector3(size, 0f, 0f), color, -Vector3.UnitY, new Vector2(1, 0)),
            new(new Vector3(size, 0f, size), color, -Vector3.UnitY, new Vector2(1, 1)),
            new(new Vector3(0f, 0f, size), color, -Vector3.UnitY, new Vector2(0, 1)),

            // Верхняя грань
            new(new Vector3(0f, height, 0f), color, Vector3.UnitY, new Vector2(0, 0)),
            new(new Vector3(0f, height, size), color, Vector3.UnitY, new Vector2(0, 1)),
            new(new Vector3(size, height, size), color, Vector3.UnitY, new Vector2(1, 1)),
            new(new Vector3(size, height, 0f), color, Vector3.UnitY, new Vector2(1, 0))
        };
    }

    public static int[] GetBlockVerticesOrder()
    {
        return [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23];
    }

    public static List<VertexElement> GetGlobalBoxVertices(float skyHeight, Color4 upSideColor, Color4 bottomSideColor)
    {
        var halfSizeX = Labyrinth.Field.GetLength(0) / 2f;
        var halfSizeZ = Labyrinth.Field.GetLength(1) / 2f;

        float texScale = 10.0f;
        return
        [
            new(new Vector3(-halfSizeX, 0f, halfSizeZ), bottomSideColor, Vector3.UnitY, new Vector2(0, 0)),
            new(new Vector3(halfSizeX, 0f, halfSizeZ), bottomSideColor, Vector3.UnitY,
                new Vector2(halfSizeX / texScale, 0)),
            new(new Vector3(halfSizeX, 0f, -halfSizeZ), bottomSideColor, Vector3.UnitY,
                new Vector2(halfSizeX / texScale, halfSizeZ / texScale)),
            new(new Vector3(-halfSizeX, 0f, -halfSizeZ), bottomSideColor, Vector3.UnitY,
                new Vector2(0, halfSizeZ / texScale)),

            // Потолок (на него накладываем текстуру)
            new(new Vector3(-halfSizeX, skyHeight, halfSizeZ), upSideColor, -Vector3.UnitY, new Vector2(0, 0)),
            new(new Vector3(halfSizeX, skyHeight, halfSizeZ), upSideColor, -Vector3.UnitY,
                new Vector2(halfSizeX / texScale, 0)),
            new(new Vector3(halfSizeX, skyHeight, -halfSizeZ), upSideColor, -Vector3.UnitY,
                new Vector2(halfSizeX / texScale, halfSizeZ / texScale)),
            new(new Vector3(-halfSizeX, skyHeight, -halfSizeZ), upSideColor, -Vector3.UnitY,
                new Vector2(0, halfSizeZ / texScale)),

            // Передняя стена
            new(new Vector3(-halfSizeX, 0f, halfSizeZ), upSideColor, -Vector3.UnitZ, new Vector2(0, 0)),
            new(new Vector3(halfSizeX, 0f, halfSizeZ), upSideColor, -Vector3.UnitZ,
                new Vector2(halfSizeX / texScale, 0)),
            new(new Vector3(halfSizeX, skyHeight, halfSizeZ), upSideColor, -Vector3.UnitZ,
                new Vector2(halfSizeX / texScale, skyHeight / texScale)),
            new(new Vector3(-halfSizeX, skyHeight, halfSizeZ), upSideColor, -Vector3.UnitZ,
                new Vector2(0, skyHeight / texScale)),

            // Левая стена
            new(new Vector3(-halfSizeX, 0f, halfSizeZ), upSideColor, Vector3.UnitX, new Vector2(0, 0)),
            new(new Vector3(-halfSizeX, 0f, -halfSizeZ), upSideColor, Vector3.UnitX,
                new Vector2(halfSizeZ / texScale, 0)),
            new(new Vector3(-halfSizeX, skyHeight, -halfSizeZ), upSideColor, Vector3.UnitX,
                new Vector2(halfSizeZ / texScale, skyHeight / texScale)),
            new(new Vector3(-halfSizeX, skyHeight, halfSizeZ), upSideColor, Vector3.UnitX,
                new Vector2(0, skyHeight / texScale)),

            // Задняя стена
            new(new Vector3(-halfSizeX, 0f, -halfSizeZ), upSideColor, Vector3.UnitZ, new Vector2(0, 0)),
            new(new Vector3(halfSizeX, 0f, -halfSizeZ), upSideColor, Vector3.UnitZ,
                new Vector2(halfSizeX / texScale, 0)),
            new(new Vector3(halfSizeX, skyHeight, -halfSizeZ), upSideColor, Vector3.UnitZ,
                new Vector2(halfSizeX / texScale, skyHeight / texScale)),
            new(new Vector3(-halfSizeX, skyHeight, -halfSizeZ), upSideColor, Vector3.UnitZ,
                new Vector2(0, skyHeight / texScale)),

            // Правая стена
            new(new Vector3(halfSizeX, 0f, halfSizeZ), upSideColor, -Vector3.UnitX, new Vector2(0, 0)),
            new(new Vector3(halfSizeX, 0f, -halfSizeZ), upSideColor, -Vector3.UnitX,
                new Vector2(halfSizeZ / texScale, 0)),
            new(new Vector3(halfSizeX, skyHeight, -halfSizeZ), upSideColor, -Vector3.UnitX,
                new Vector2(halfSizeZ / texScale, skyHeight / texScale)),
            new(new Vector3(halfSizeX, skyHeight, halfSizeZ), upSideColor, -Vector3.UnitX,
                new Vector2(0, skyHeight / texScale))
        ];
    }

    public static int[] GetGlobalBoxVerticesOrder()
    {
        return [0, 1, 2, 3, 7, 6, 5, 4, 11, 10, 9, 8, 12, 13, 14, 15, 16, 17, 18, 19, 23, 22, 21, 20];
    }
}