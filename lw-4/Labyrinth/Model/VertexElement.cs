using OpenTK.Mathematics;
using Color4 = OpenTK.Mathematics.Color4;

public struct VertexElement
{
    public Vector3 Position;
    public Color4 Color;
    public Vector3 Normal;
    public Vector2 TexCoord;

    public const int PositionIndex = 0;
    public const int ColorIndex = 3;
    public const int NormalIndex = 7;
    public const int TexCoordIndex = 10;
    public const int Size = 12; // Увеличили размер, добавив TexCoord

    public VertexElement(Vector3 position, Color4 color = default, Vector3 normal = default, Vector2 texCoord = default)
    {
        Position = position;
        Color = color == default ? Color4.Black : color;
        Normal = normal == default ? Vector3.UnitY : normal;
        TexCoord = texCoord == default ? Vector2.Zero : texCoord;
    }
    
    public float[] ToArray()
    {
        return
        [
            Position.X, Position.Y, Position.Z,
            Color.R, Color.G, Color.B, Color.A,
            Normal.X, Normal.Y, Normal.Z,
            TexCoord.X, TexCoord.Y
        ];
    }
}