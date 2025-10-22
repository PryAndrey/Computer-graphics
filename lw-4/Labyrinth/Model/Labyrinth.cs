using OpenTK.Mathematics;
using PrimitiveType = OpenTK.Graphics.OpenGL4.PrimitiveType;

public class Labyrinth
{
    private static readonly Color4 BlockColor = Color4.Gray;
    private static readonly Color4 SkyColor = Color4.Bisque;
    private static readonly Color4 GroundColor = Color4.ForestGreen;

    public const float Size = 1f;
    public const float Height = 1f;
    public const float SkyHeight = 4.2f;

    private readonly float[] _blockVertices;
    private readonly float[] _boxVertices;

    public readonly Tuple<Vector3, float>[] BlockPositions;

    // todo Разные блоки - OK
    // todo из позици камеры вычитать 28 строка .frag - OK
    // todo Для нормали взять 3 на 3 из матрицы модели инвертировать транспонировать .vert - OK
    
    // todo Привязать skybox к наблюдателю
    // todo Mitmapping
    public static readonly float[,] Field = new float[,]
    {
        { 3, 6, 2, 0, 7, 5, 4, 8, 1, 3, 7, 2, 6, 4, 5, 8 },
        { 2, 0, 0, 0, 1, 0, 0, 0, 0, 0, 8, 0, 0, 0, 0, 4 },
        { 5, 0, 7, 0, 3, 2, 8, 1, 6, 0, 4, 0, 7, 5, 0, 1 },
        { 1, 0, 3, 0, 0, 0, 0, 0, 8, 0, 2, 0, 0, 6, 0, 7 },
        { 4, 0, 5, 1, 2, 7, 6, 0, 3, 0, 1, 8, 0, 0, 0, 2 },
        { 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 3, 0, 5 },
        { 8, 1, 7, 2, 5, 0, 3, 0, 4, 6, 0, 1, 0, 2, 0, 3 },
        { 3, 0, 0, 0, 7, 0, 0, 0, 0, 8, 0, 5, 0, 4, 0, 6 },
        { 2, 0, 6, 0, 1, 0, 0, 5, 0, 7, 0, 0, 0, 0, 0, 8 },
        { 4, 0, 8, 0, 3, 0, 0, 0, 0, 2, 0, 7, 0, 1, 0, 4 },
        { 5, 0, 1, 0, 6, 8, 0, 4, 0, 3, 0, 2, 0, 5, 0, 1 },
        { 7, 0, 2, 0, 0, 3, 0, 6, 0, 5, 0, 8, 0, 7, 0, 3 },
        { 1, 0, 4, 6, 0, 2, 0, 7, 0, 4, 8, 3, 0, 6, 0, 2 },
        { 8, 0, 0, 5, 0, 1, 0, 3, 0, 0, 0, 0, 0, 8, 0, 7 },
        { 2, 3, 0, 4, 0, 7, 0, 0, 5, 2, 6, 8, 0, 0, 0, 5 },
        { 6, 7, 5, 1, 8, 4, 2, 3, 7, 6, 1, 5, 2, 8, 3, 4 }
    };

    public Labyrinth()
    {
        var blockVerticesList = LabyrinthTextures.GetVerticesList(Height, Size, BlockColor);
        _blockVertices = blockVerticesList.SelectMany(vert => vert.ToArray()).ToArray();

        BlockPositions = BlockPositionsToArray();

        var boxVerticesList = LabyrinthTextures.GetGlobalBoxVertices(SkyHeight, SkyColor, GroundColor);
        _boxVertices = boxVerticesList.SelectMany(vert => vert.ToArray()).ToArray();
    }

    public void Draw(Renderer renderer)
    {
        foreach (var position in BlockPositions)
        {
            renderer.DrawElements(PrimitiveType.Quads, _blockVertices, LabyrinthTextures.GetBlockVerticesOrder(),
                position.Item1, position.Item2);
        }

        renderer.DrawElements(PrimitiveType.Quads, _boxVertices, LabyrinthTextures.GetGlobalBoxVerticesOrder(),
            Vector3.Zero, 9); // Используем текстуру с ID 1
    }

    public static Tuple<Vector3, float>[] BlockPositionsToArray()
    {
        var blockPositionsList = new List<Tuple<Vector3, float>>();
        var centerX = Field.GetLength(0) / 2f;
        var centerZ = Field.GetLength(1) / 2f;

        for (int row = 0; row < Field.GetLength(0); row++)
        {
            for (int column = 0; column < Field.GetLength(1); column++)
            {
                var block = Field[row, column];
                if (block == 0) continue;

                var position = new Vector3(row - centerX, 0, column - centerZ);
                var tuple = Tuple.Create(position, block);

                blockPositionsList.Add(tuple);
            }
        }

        return blockPositionsList.ToArray();
    }
}