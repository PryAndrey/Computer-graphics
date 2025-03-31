using OpenTK.Mathematics;

namespace Field.Models;

public interface ISceneObject
{
    void Update(float deltaTime);

    void Render(ICanvas canvas);

    void Resize(float originalWidth, float originalHeight, float widthNew, float heightNew);
}

public enum ObjectType
{
    Butterfly,
    Flower,
    Grass,
    Cloud,
    Sun,
    Moon,
    Sky
}

public abstract class SceneObject : ISceneObject
{
    public ObjectType Type { get; set; }
    protected Vector2 Position;
    protected Vector2 OriginalPosition;
    protected Vector2 OriginalSize;
    protected Vector2 Size;

    public virtual void Resize(float originalWidth, float originalHeight, float widthNew, float heightNew)
    {
        Position.X = OriginalPosition.X / originalWidth * Math.Min(widthNew, heightNew);
        Position.Y = OriginalPosition.Y / originalHeight * Math.Min(widthNew, heightNew);
        Size.X = OriginalSize.X / originalWidth * Math.Min(widthNew, heightNew);
        Size.Y = OriginalSize.Y / originalHeight * Math.Min(widthNew, heightNew);
    }

    public abstract void Update(float deltaTime);

    public abstract void Render(ICanvas canvas);
}