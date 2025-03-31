using MeadowScene.Models;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Field.Models;

public class Flower : SceneObject
{
    private Color4 _color;

    public Flower(Vector2 position, Color4 color)
    {
        Type = ObjectType.Flower;
        Position = position;
        _color = color;
        Size = new Vector2(40, 40);
        OriginalPosition = Position;
        OriginalSize = Size;
    }

    public override void Update(float deltaTime)
    {
    }

    public override void Render(ICanvas canvas)
    {
        canvas.DrawLine(Position.X, Position.Y, Position.X, Position.Y - Size.Y,
            Color4.Green.R, Color4.Green.G, Color4.Green.B, Color4.Green.A);

        canvas.DrawCircle(Position.X, Position.Y, Size.X * 0.25f, Size.Y * 0.25f, 360,
            Color4.Yellow.R, Color4.Yellow.G, Color4.Yellow.B, Color4.Yellow.A);
        
        int petalCount = 6;
        float petalRadiusX = Size.X * 0.17f;
        float petalRadiusY = Size.Y * 0.17f; 
        float petalDistance = Size.X / 3;

        for (int i = 0; i < petalCount; i++)
        {
            double angle = Math.PI * 2 * i / petalCount;
            float petalX = Position.X + (float)Math.Cos(angle) * petalDistance;
            float petalY = Position.Y + (float)Math.Sin(angle) * petalDistance;

            canvas.DrawCircle(petalX, petalY, petalRadiusX,petalRadiusY, 360,
                _color.R, _color.G, _color.B, _color.A);
        }
    }
}