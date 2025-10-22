using System.Drawing;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;

public class ViewWindow : GameWindow
{
    private Renderer _renderer;

    private Shader _shader;

    private Camera _camera;

    private bool _firstMove = true;

    private readonly Vector3 _initCameraPosition = new(0.0f, 0.0f, 10.5f);

    private Vector2 _lastPos;

    public ViewWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(Color.White);

        GL.Enable(EnableCap.ProgramPointSize);
        GL.Enable(EnableCap.CullFace);
        
        _shader = new Shader("../../../Model/Shaders/shader.vert", "../../../Model/Shaders/shader.frag");
        _camera = new Camera(_initCameraPosition, Size.X / (float)Size.Y, (float)Math.PI, (float)Math.PI);

        _renderer = new Renderer(_shader);
    }
    
    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _renderer.DrawElements();

        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }
    }
    
    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, Size.X, Size.Y);
        _camera.AspectRatio = Size.X / (float)Size.Y;
    }
}