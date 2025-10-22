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

    private Labyrinth _labyrinth;
    
    private MovesModule _movesModule;

    private bool _firstMove = true;

    private readonly Vector3 _lightColor = new(1.0f, 1.0f, 1.0f);
    private readonly Vector3 _initCameraPosition = new(0.5f, 0.6f, 0.5f);

    private Vector2 _lastPos;

    public ViewWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0, 0, 0, 0);

        GL.Enable(EnableCap.DepthTest);
        GL.Enable(EnableCap.CullFace);
        
        _shader = new Shader("../../../Model/Shaders/shader.vert", "../../../Model/Shaders/shader.frag");
        _camera = new Camera(_initCameraPosition, Size.X / (float)Size.Y, (float)Math.PI, (float)Math.PI);

        _renderer = new Renderer(_shader);

        _labyrinth = new Labyrinth();
        
        _movesModule = new MovesModule(_camera, _labyrinth);

        CursorState = CursorState.Grabbed;
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _shader.SetMatrix4("view", _camera.GetViewMatrix());
        _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());

        _shader.SetVector3("lightColor", _lightColor);
        _shader.SetVector3("cameraPos", _camera.Position);
        
        _shader.SetVector3("fogColor", new Vector3(1.0f, 1.0f, 1.0f));
        _shader.SetFloat("fogDensity", 0.15f);

        _labyrinth.Draw(_renderer);

        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }
        
        _movesModule.MoveProcess(KeyboardState, (float)e.Time);

        _movesModule.MouseProcess(MouseState, ref _lastPos, ref _firstMove);
    }
    
    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, Size.X, Size.Y);
        _camera.AspectRatio = Size.X / (float)Size.Y;
    }
}