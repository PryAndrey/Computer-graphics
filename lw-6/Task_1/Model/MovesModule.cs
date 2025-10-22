using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

public class MovesModule
{
    private bool _firstMove = true;
    private Vector2 _lastPos;

    private Camera _camera;

    public MovesModule(Camera camera)
    {
        _camera = camera;
    }

    public void MoveProcess(KeyboardState keyboardState)
    {
    }

    public void MouseProcess(MouseState mouseState)
    {
        if (_firstMove)
        {
            _lastPos = new Vector2(mouseState.X, mouseState.Y);
            _firstMove = false;
        }
        else
        {
            var deltaX = mouseState.X - _lastPos.X;
            var deltaY = mouseState.Y - _lastPos.Y;
            _lastPos = new Vector2(mouseState.X, mouseState.Y);

            _camera.Yaw += deltaX * Camera.Sensitivity;
            _camera.Pitch -= deltaY * Camera.Sensitivity;
        }
    }

    public void WheelProcess(float deltaY)
    {
        if (deltaY < 0)
            _camera.distance += 0.5f;
        else
            _camera.distance = MathF.Max(_camera.distance - 0.5f, 1.0f);
    }
}