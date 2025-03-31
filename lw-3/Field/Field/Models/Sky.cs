using Field.Models;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace MeadowScene.Models;

public class Sky : SceneObject
{
    private Color4 _color;
    private Color4 _starColor;
    private bool _down = true;
    private float _speed = 0.09f;
    private List<Vector2> _stars;
    private List<Vector2> _originalStars;
    private Random _random;

    public Sky(Vector2 position)
    {
        Type = ObjectType.Sky;
        Position = position;
        OriginalPosition = Position;
        _color = Color4.SkyBlue;
        _starColor = Color4.SkyBlue;
        _random = new Random();
        _originalStars = GenerateRandomPoints(60, 0, 600, 0, 600);
        _stars = new List<Vector2>(_originalStars);
    }

    public List<Vector2> GenerateRandomPoints(int count, float minX, float maxX, float minY, float maxY)
    {
        List<Vector2> points = new List<Vector2>();

        for (int i = 0; i < count; i++)
        {
            float x = (float)(_random.NextDouble() * (maxX - minX) + minX);
            float y = (float)(_random.NextDouble() * (maxY - minY) + minY);
            points.Add(new Vector2(x, y));
        }

        return points;
    }

    public override void Update(float deltaTime)
    {
        _color = new Color4(
            _color.R + (_down ? -_speed : _speed) * deltaTime,
            _color.G + (_down ? -_speed : _speed) * deltaTime,
            _color.B + (_down ? -_speed : _speed) * deltaTime,
            _color.A + (_down ? -_speed : _speed) * deltaTime);
        _starColor = new Color4(
            _starColor.R + (_down ? _speed : -_speed) * deltaTime,
            _starColor.G + (_down ? _speed : -_speed) * deltaTime,
            _starColor.B + (_down ? _speed : -_speed) * deltaTime,
            _starColor.A + (_down ? _speed : -_speed) * deltaTime);

        if (_color.A >= Color4.SkyBlue.A)
        {
            _down = true;
        }

        if (_color.A < 0)
        {
            _down = false;
        }
    }

    public override void Render(ICanvas canvas)
    {
        canvas.ClearScreen( _color.R, _color.G, _color.B, _color.A);
        
        foreach (var star in _stars)
        {
            canvas.DrawCircle(star.X, star.Y, 3, 3, 30,
                _starColor.R, _starColor.G, _starColor.B, _starColor.A);
        }

        canvas.DrawCircle(Position.X / 2, Position.Y / -1.3f, Position.X, Position.Y, 360,
            Color4.LightGreen.R, Color4.LightGreen.G, Color4.LightGreen.B, Color4.LightGreen.A);
    }

    public override void Resize(float originalWidth, float originalHeight, float widthNew, float heightNew)
    {
        Position.X = OriginalPosition.X / originalWidth * Math.Min(widthNew, heightNew);
        Position.Y = OriginalPosition.Y / originalHeight * Math.Min(widthNew, heightNew);
        Size.X = OriginalSize.X / originalWidth * Math.Min(widthNew, heightNew);
        Size.Y = OriginalSize.Y / originalHeight * Math.Min(widthNew, heightNew);
        for (int i = 0; i < _originalStars.Count(); i++)
        {
            var point = _originalStars[i];

            point.X = point.X / originalWidth * Math.Min(widthNew, heightNew);
            point.Y = point.Y / originalHeight * Math.Min(widthNew, heightNew);

            _stars[i] = point;
        }
    }
}