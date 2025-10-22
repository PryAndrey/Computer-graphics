using OpenTK.Mathematics;

public class Camera
{
    private Vector3 _front = -Vector3.UnitZ;

    private Vector3 _up = Vector3.UnitY;

    private Vector3 _right = Vector3.UnitX;

    private float _pitch;

    private float _yawing = -MathHelper.PiOver2;

    private readonly float _fieldOfView = MathHelper.PiOver2;

    public float Speed { get; set; } = 2.0f;
    public const float Sensitivity = 0.25f;

    public Camera(Vector3 position, float aspectRatio, float vertAngle, float horAngle)
    {
        Position = position;
        AspectRatio = aspectRatio;
        _pitch = MathHelper.DegreesToRadians(vertAngle);
        _yawing = MathHelper.DegreesToRadians(horAngle);
    }

    public Vector3 Position { get; set; }

    public float AspectRatio { private get; set; }

    public Vector3 Front => _front;

    public Vector3 Up => _up;

    public Vector3 Right => _right;

    public float Pitch
    {
        get => MathHelper.RadiansToDegrees(_pitch);
        set
        {
            var angle = MathHelper.Clamp(value, -89f, 89f);
            _pitch = MathHelper.DegreesToRadians(angle);
            UpdateVectors();
        }
    }

    public float Yaw
    {
        get => MathHelper.RadiansToDegrees(_yawing);
        set
        {
            _yawing = MathHelper.DegreesToRadians(value);
            UpdateVectors();
        }
    }

    public Matrix4 GetViewMatrix()
    {
        return Matrix4.LookAt(Position, Position + _front, _up);
    }

    public Matrix4 GetProjectionMatrix()
    {
        return Matrix4.CreatePerspectiveFieldOfView(_fieldOfView, AspectRatio, 0.01f, 100f);
    }

    private void UpdateVectors()
    {
        _front.X = MathF.Cos(_pitch) * MathF.Cos(_yawing);
        _front.Y = MathF.Sin(_pitch);
        _front.Z = MathF.Cos(_pitch) * MathF.Sin(_yawing);

        _front = Vector3.Normalize(_front);

        _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
        _up = Vector3.Normalize(Vector3.Cross(_right, _front));
    }
}