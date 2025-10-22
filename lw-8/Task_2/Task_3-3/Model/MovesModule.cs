using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

public class MovesModule
{

    private Camera _camera;

    private bool _firstMove = true;
    public MovesModule(Camera camera)
    {
        _camera = camera;
    }
    
    public void MouseProcess(MouseState mouseState, ref Vector2 lastPos)
    {
        if (_firstMove)
        {
            lastPos = new Vector2(mouseState.X, mouseState.Y);
            _firstMove = false;
        }
        else
        {
            var deltaX = mouseState.X - lastPos.X;
            var deltaY = mouseState.Y - lastPos.Y;
            lastPos = new Vector2(mouseState.X, mouseState.Y);

            _camera.Yaw += deltaX * Camera.Sensitivity;
            _camera.Pitch -= deltaY * Camera.Sensitivity;
        }
    }
    
    public void WheelProcess(float deltaY)
    {
        if (deltaY < 0)
            _camera.distance += 0.2f;
        else
            _camera.distance = MathF.Max(_camera.distance - 0.2f, 0.01f);
    }
}