using MobiusStrip.Utilities;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MobiusStrip;

partial class ViewWindow : GameWindow
{
    private MobiusStrip _mobiusStrip;

    private RotationCamera _rotationCamera;

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
        _rotationCamera = new RotationCamera(new Vector3(0f, 0f, 5f), (float)Size.X / Size.Y);
        _renderer = new Renderer(_rotationCamera);
        _mobiusStrip = new MobiusStrip();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _mobiusStrip.Draw(_renderer, Vector3.Zero);

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
        _rotationCamera.Rotate(MousePosition);
    }

    private void ProcessKeyboard(double time)
    {
        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, Size.X, Size.Y);

        _rotationCamera.AspectRatio = Size.X / (float)Size.Y;
    }

    protected override void OnUnload()
    {
        base.OnUnload();

        _renderer.Dispose();
    }
}