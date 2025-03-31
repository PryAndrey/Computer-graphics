using MeadowScene.Models;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Field.Models;

public class Grass : SceneObject
{
    private Color4 _color;

    public Grass(Vector2 position, Color4 color)
    {
        Type = ObjectType.Grass;
        Position = position;
        _color = color;
        Size = new Vector2(10, 50);
        OriginalPosition = Position;
        OriginalSize = Size;
    }

    public override void Update(float deltaTime)
    {
    }

    public override void Render(ICanvas canvas)
    {
        for (int i = -3; i <= 3; i++)
        {
            float offset = i * 7;
            canvas.DrawTriangle(
                Position.X + offset, Position.Y,
                Position.X + Size.X + offset, Position.Y,
                Position.X + Size.X / (i % 2 == 0 ? 2 : 3) + offset, Position.Y + Size.Y * (i % 2 == 0 ? 1 : 1.2f),
                _color.R, _color.G, _color.B, _color.A);
        }

        canvas.DrawQuadGradient(
            Position.X - 3 * 8, Position.Y - 10,
            Position.X + Size.X + 3 * 8, Position.Y - 10,
            Position.X + Size.X + 3 * 7, Position.Y,
            Position.X - 3 * 7, Position.Y,
            Color4.LightGreen.R, Color4.LightGreen.G, Color4.LightGreen.B, Color4.LightGreen.A,
            _color.R, _color.G, _color.B, _color.A
            );
    }
}