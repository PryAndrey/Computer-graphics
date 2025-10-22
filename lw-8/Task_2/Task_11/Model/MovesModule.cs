using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

public class MovesModule
{
    private readonly Camera _camera;
    private bool _firstMove = true;
    private Vector2 _lastPos;

    public MovesModule(Camera camera) => _camera = camera;

    public void MouseProcess(MouseState mouseState)
    {
        if (_firstMove)
        {
            _lastPos = new Vector2(mouseState.X, mouseState.Y);
            _firstMove = false;
        }
        else
        {
            var delta = new Vector2(
                (mouseState.X - _lastPos.X) * 0.002f,
                (mouseState.Y - _lastPos.Y) * 0.002f);
            
            _camera.Rotate(delta.X, delta.Y);
            _lastPos = new Vector2(mouseState.X, mouseState.Y);
        }
    }

    public void WheelProcess(float delta) => _camera.Zoom(delta * 0.1f);

}