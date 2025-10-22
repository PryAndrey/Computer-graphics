using OpenTK.Mathematics;

public class Camera
{
    private float _distance = 5f;
    private float _azimuth = 0f; // Горизонтальный угол
    private float _polar = MathHelper.PiOver4; // Вертикальный угол (45 градусов)

    public Vector3 Position { get; private set; }
    public Vector3 Target { get; set; } = new Vector3(0f, 0.5f, -5f); // Центр на кубе
    public Vector3 Up { get; } = Vector3.UnitY;

    public void Update()
    {
        // Сферические координаты в декартовы
        Position = new Vector3(
            Target.X + _distance * MathF.Sin(_polar) * MathF.Cos(_azimuth),
            Target.Y + _distance * MathF.Cos(_polar),
            Target.Z + _distance * MathF.Sin(_polar) * MathF.Sin(_azimuth)
        );
    }

    public void Rotate(float deltaAzimuth, float deltaPolar)
    {
        _azimuth += deltaAzimuth;
        _polar = Math.Clamp(_polar + deltaPolar, 0.1f, MathHelper.Pi - 0.1f);
        Update();
    }

    public void Zoom(float delta)
    {
        _distance = Math.Clamp(_distance - delta, 1f, 20f);
        Update();
    }

    public Matrix4 GetViewMatrix() => Matrix4.LookAt(Position, Target, Up);
}