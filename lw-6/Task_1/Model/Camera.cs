using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

public class Camera
{
    private Vector3 _front = -Vector3.UnitZ;

    private float _pitch;
    private float _yawing;

    private readonly float _fieldOfView = MathHelper.PiOver2;

    public const float Sensitivity = 0.25f;
    public float distance = 3f;

    public Camera(float aspectRatio, float vertAngle, float horAngle)
    {
        AspectRatio = aspectRatio;
        _pitch = MathHelper.DegreesToRadians(vertAngle);
        _yawing = MathHelper.DegreesToRadians(horAngle);
    }

    public Vector3 Position { get; set; }
    public float AspectRatio { private get; set; }

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
        return Matrix4.LookAt(Position, Vector3.Zero, Vector3.UnitY);
    }

    public Matrix4 GetProjectionMatrix()
    {
        return Matrix4.CreatePerspectiveFieldOfView(_fieldOfView, AspectRatio, 0.1f, 100000f);
    }

    private void UpdateVectors()
    {
        _front.X = distance * MathF.Cos(_pitch) * MathF.Cos(_yawing);
        _front.Y = distance * MathF.Sin(_pitch);
        _front.Z = distance * MathF.Cos(_pitch) * MathF.Sin(_yawing);

        Position = _front;
    }
}