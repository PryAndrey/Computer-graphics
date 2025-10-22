using OpenTK.Mathematics;
using Color4 = OpenTK.Mathematics.Color4;

public struct VertexElement
{
    public Vector3 Position;
    public Color4 Color;
    public Vector3 Normal;

    public const int PositionIndex = 0;
    public const int ColorIndex = 3;
    public const int NormalIndex = 7;
    public const int Size = 10; 

    public VertexElement(Vector3 position, Color4 color = default)
    {
        Position = position;
        Color = color == default ? Color4.Black : color;
        Normal = Vector3.Zero; 
    }
    
    public float[] ToArray()
    {
        return new float[]
        {
            Position.X, Position.Y, Position.Z,
            Color.R, Color.G, Color.B, Color.A,
            Normal.X, Normal.Y, Normal.Z
        };
    }
}