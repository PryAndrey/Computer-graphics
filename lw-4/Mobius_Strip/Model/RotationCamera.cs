using OpenTK.Mathematics;

public class RotationCamera
{
    private Vector2 _lastPosition;

    private const float Sensitivity = 0.1f;

    public RotationCamera(Vector3 position, float aspectRatio)
    {
        _lastPosition = new Vector2(position.X, position.Y);
        Position = position;
        AspectRatio = aspectRatio;
    }

    public float X { get; private set; }
    public float Y { get; private set; }

    public float AspectRatio { private get; set; }

    public Vector3 Position { get; set; }

    public void Rotate(Vector2 newPosition)
    {
        float deltaX = newPosition.X - _lastPosition.X;
        float deltaY = newPosition.Y - _lastPosition.Y;

        Y += deltaX * Sensitivity;
        X -= deltaY * Sensitivity;

        _lastPosition = newPosition;
    }

    public Matrix4 GetViewMatrix()
    {
        var rotationMatrixX = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(X));
        var rotationMatrixY = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Y));
// todo поправить нормирование для ротации
        var viewMatrix = rotationMatrixY * rotationMatrixX * Matrix4.LookAt(Position, Vector3.Zero, Vector3.UnitY);

        return viewMatrix;
    }

    public Matrix4 GetProjectionMatrix()
    {
        return Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, AspectRatio, 0.1f, 100f);
    }
}