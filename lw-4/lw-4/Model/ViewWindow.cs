using Cuboctahedron.Utilities;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Cuboctahedron;

public class ViewWindow : GameWindow
{
    private Figure _figure;

    private Camera _camera;

    private bool _firstMove = true;

    private Vector2 _lastPos;

    private Renderer _renderer;

    public ViewWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        CursorState = CursorState.Grabbed;
        GL.ClearColor(0.10f, 0.10f, 0.10f, 1.0f);
        GL.Enable(EnableCap.DepthTest);
        _camera = new Camera(Vector3.UnitZ * 2 + Vector3.UnitY * 2 + Vector3.UnitX * 2, Size.X / (float)Size.Y , -45, -135);
        _renderer = new Renderer();
        _figure = new Figure();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _renderer.SetViewMatrix(_camera.GetViewMatrix());
        _renderer.SetProjectionMatrix(_camera.GetProjectionMatrix());
        _renderer.SetLightPosition(new Vector3(0, 15, 10)); 

        GL.Enable(EnableCap.CullFace);
        _figure.Draw(_renderer, Vector3.Zero);

        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);
        ProcessKeyboard(e.Time);
        ProcessMouseMovement();
    }

    private void ProcessMouseMovement()
    {
        if (_firstMove)
        {
            _lastPos = new Vector2(MouseState.X, MouseState.Y);
            _firstMove = false;
        }
        else
        {
            var deltaX = MouseState.X - _lastPos.X;
            var deltaY = MouseState.Y - _lastPos.Y;
            _lastPos = new Vector2(MouseState.X, MouseState.Y);

            _camera.Yaw += deltaX * Camera.Sensitivity;
            _camera.Pitch -= deltaY * Camera.Sensitivity;
        }
    }

    private void ProcessKeyboard(double time)
    {
        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }

        if (KeyboardState.IsKeyDown(Keys.W))
        {
            _camera.Position += _camera.Front * _camera.Speed * (float)time;
        }

        if (KeyboardState.IsKeyDown(Keys.S))
        {
            _camera.Position -= _camera.Front * _camera.Speed * (float)time;
        }

        if (KeyboardState.IsKeyDown(Keys.A))
        {
            _camera.Position -= _camera.Right * _camera.Speed * (float)time;
        }

        if (KeyboardState.IsKeyDown(Keys.D))
        {
            _camera.Position += _camera.Right * _camera.Speed * (float)time;
        }

        if (KeyboardState.IsKeyDown(Keys.Space))
        {
            _camera.Position += _camera.Up * _camera.Speed * (float)time;
        }

        if (KeyboardState.IsKeyDown(Keys.LeftShift))
        {
            _camera.Position -= _camera.Up * _camera.Speed * (float)time;
        }

        if (KeyboardState.IsKeyDown(Keys.LeftControl))
        {
            _camera.Speed = Math.Abs(_camera.Speed - 2.0f) < 0.001f ? 10.0f : 2.0f;
        }
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, Size.X, Size.Y);

        _camera.AspectRatio = Size.X / (float)Size.Y;
    }

    protected override void OnUnload()
    {
        base.OnUnload();

        _renderer.Dispose();
    }
}