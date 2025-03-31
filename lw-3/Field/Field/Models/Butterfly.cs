using MeadowScene.Models;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Field.Models;

public class Butterfly : SceneObject
{
    private Color4 _color;
    private float _delay = 1;
    private int _currentAim = 0;
    private List<Vector2> _points = new List<Vector2>();
    private List<Vector2> _originalPoints = new List<Vector2>();

    public Butterfly(Vector2 position, Color4 color)
    {
        Type = ObjectType.Butterfly;
        Position = position;
        OriginalPosition = position;
        _color = color;
        Size = new Vector2(40, 40);
        OriginalPosition = Position;
        OriginalSize = Size;
    }

    public override void Update(float deltaTime)
    {
        _delay += deltaTime;
        if (_delay < 1)
        {
            return;
        }

        if (Math.Abs(Position.X - _points[_currentAim].X) < 2 &&
            Math.Abs(Position.Y - _points[_currentAim].Y) < 2)
        {
            _delay = 0;
            _currentAim = _currentAim < _points.Count - 1 ? ++_currentAim : 0;
        }

        MoveTowards(_points[_currentAim].X, _points[_currentAim].Y, 2);
    }

    public void SetFlowers(List<Vector2> points)
    {
        _points = new List<Vector2>(points);
        _originalPoints = new List<Vector2>(points);
    }

    public void MoveTowards(float targetX, float targetY, float stepSize)
    {
        float deltaX = targetX - Position.X;
        float deltaY = targetY - Position.Y;

        double angle = Math.Atan2(deltaY, deltaX) + 0.5;

        float newX = Position.X + (float)(Math.Cos(angle) * stepSize);
        float newY = Position.Y + (float)(Math.Sin(angle) * stepSize);

        Position.X = newX;
        Position.Y = newY;
    }

    public override void Render(ICanvas canvas)
    {
        canvas.DrawTriangle(
            Position.X - 1, Position.Y - 1,
            Position.X - 1, Position.Y + Size.Y * 0.8f,
            Position.X - Size.X / 2, Position.Y + Size.Y * 0.6f,
            _color.R, _color.G, _color.B, _color.A);

        canvas.DrawLine(
            Position.X, Position.Y + Size.Y * 0.8f,
            Position.X + Size.X * 0.3f, Position.Y + Size.Y,
            Color4.Chocolate.R, Color4.Chocolate.G, Color4.Chocolate.B, Color4.Chocolate.A);

        canvas.DrawTriangle(
            Position.X + 1, Position.Y,
            Position.X + Size.X / 2, Position.Y + Size.Y * 0.6f,
            Position.X + 1, Position.Y + Size.Y * 0.8f,
            _color.R, _color.G, _color.B, _color.A);

        canvas.DrawLine(
            Position.X, Position.Y + Size.Y * 0.8f,
            Position.X - Size.X * 0.3f, Position.Y + Size.Y,
            Color4.Chocolate.R, Color4.Chocolate.G, Color4.Chocolate.B, Color4.Chocolate.A);
    }


    public override void Resize(float originalWidth, float originalHeight, float widthNew, float heightNew)
    {
        Position.X = OriginalPosition.X / originalWidth * Math.Min(widthNew, heightNew);
        Position.Y = OriginalPosition.Y / originalHeight * Math.Min(widthNew, heightNew);
        Size.X = OriginalSize.X / originalWidth * Math.Min(widthNew, heightNew);
        Size.Y = OriginalSize.Y / originalHeight * Math.Min(widthNew, heightNew);
        for (int i = 0; i < _originalPoints.Count(); i++)
        {
            var point = _originalPoints[i];

            point.X = point.X / originalWidth * Math.Min(widthNew, heightNew);
            point.Y = point.Y / originalHeight * Math.Min(widthNew, heightNew);

            _points[i] = point;
        }

        Size.Y =Size.Y;
    }
}