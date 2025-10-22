using System.Drawing;
using MobiusStrip.Model;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using Task2.Shaders;

// todo находить пересечение для подсчета нормали(не шагать)
public class ViewWindow : GameWindow
{
    private Shader _shader;

    private Camera _camera;

    private MyScene _myScene;

    private MovesModule _movesModule;

    private readonly Vector3 _lightColor = new(1.0f, 1.0f, 1.0f);
    private readonly Vector3 _lightPos = new(3f, 3f, 0f);

    private Vector2 _lastPos;

    private Renderer _renderer;

    public ViewWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(Color.White);
        
        GL.Enable(EnableCap.DepthTest);
        
        CursorState = CursorState.Grabbed;

        _shader = new Shader("../../../Model/Shaders/shader.vert", "../../../Model/Shaders/shader.frag");

        _camera = new Camera(Size.X / (float)Size.Y, (float)Math.PI, (float)Math.PI);

        _movesModule = new MovesModule(_camera);
        
        _renderer = new Renderer(_shader);
        
        _myScene = new MyScene();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _shader.Use();

        _shader.SetMatrix4("model", _myScene.GetModelMatrix());
        _shader.SetMatrix4("view", _camera.GetViewMatrix());
        _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
        
        _shader.SetVector3("lightPos", _lightPos);
        _shader.SetVector3("lightColor", _lightColor);
        _shader.SetVector3("cameraPos", _camera.Position);

        _myScene.Draw(_renderer);

        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }

        _movesModule.MouseProcess(MouseState, ref _lastPos);
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        base.OnMouseWheel(e);

        _movesModule.WheelProcess(e.OffsetY);
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, Size.X, Size.Y);
        _camera.AspectRatio = Size.X / (float)Size.Y;
    }
}