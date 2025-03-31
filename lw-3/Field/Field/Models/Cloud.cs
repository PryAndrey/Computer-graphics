using MeadowScene.Models;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Field.Models;

public class Cloud : SceneObject
{
    public Cloud(Vector2 position)
    {
        Type = ObjectType.Cloud;
        Position = position;
        Size = new Vector2(50, 30);
        OriginalPosition = Position;
        OriginalSize = Size;
    }

    public override void Update(float deltaTime)
    {
        Position.X += 10 * deltaTime;
        if (Position.X > 800)
        {
            Position.X = -100;
        }
    }

    public override void Render(ICanvas canvas)
    {
        canvas.DrawCircle(Position.X, Position.Y, Size.X, Size.Y, 360,
            Color4.White.R, Color4.White.G, Color4.White.B, Color4.White.A);
    }
}