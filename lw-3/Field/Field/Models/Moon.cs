using OpenTK.Mathematics;

namespace Field.Models;

public class Moon : SceneObject
{
    private float _angle = -90;
    private float _speed = 0.3f;
    private float _radius = 370;
    private Vector2 _center;

    public Moon(Vector2 position, float radius)
    {
        Type = ObjectType.Sun;
        Position = position;
        Size = new Vector2(50, 50);
        _radius = radius;
        OriginalPosition = Position;
        _center = Position;
        OriginalSize = Size;
    }

    public override void Update(float deltaTime)
    {
        _angle -= _speed * deltaTime;

        Position.X = _center.X + _radius * MathF.Cos(_angle);
        Position.Y = _center.Y + _radius * MathF.Sin(_angle);
    }

    public override void Render(ICanvas canvas)
    {
        canvas.DrawCircle(Position.X, Position.Y, Size.X, Size.Y, 360,
            Color4.Gray.R, Color4.Gray.G, Color4.Gray.B, Color4.Gray.A);
    }
    
    public override void Resize(float originalWidth, float originalHeight, float widthNew, float heightNew)
    {
        Position.X = OriginalPosition.X / originalWidth * Math.Min(widthNew, heightNew);
        Position.Y = OriginalPosition.Y / originalHeight * Math.Min(widthNew, heightNew);
        Size.X = OriginalSize.X / originalWidth * Math.Min(widthNew, heightNew);
        Size.Y = OriginalSize.Y / originalHeight * Math.Min(widthNew, heightNew);
        _center.X = widthNew / 2;
        _center.Y = 0;
        _radius = Math.Min(widthNew, heightNew) / 1.6f;
    }
}