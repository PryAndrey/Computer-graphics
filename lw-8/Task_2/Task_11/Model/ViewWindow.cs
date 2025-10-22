using System.Drawing;
using MobiusStrip.Model;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using Task2.Shaders;

public class ViewWindow : GameWindow
{
    private Shader _shader;

    private Camera _camera;

    private MyScene _myScene;

    private MovesModule _movesModule;

    private Vector2 _lastPos;

    private double _time;
    private Renderer _renderer;

    public ViewWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(Color.White);
        // GL.Enable(EnableCap.DepthTest);

        _shader = new Shader("../../../Model/Shaders/shader.vert", "../../../Model/Shaders/shader.frag");

        _camera = new Camera();
        _movesModule = new MovesModule(_camera);

        _renderer = new Renderer(_shader);
        _myScene = new MyScene();

        CursorState = CursorState.Grabbed;
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _time += e.Time;
        
        _shader.Use();

        _shader.SetFloat("time", (float)_time);
        _shader.SetVector2("resolution", new Vector2(Size.X, Size.Y));
        
        // _shader.SetMatrix4("model", _myScene.GetModelMatrix());
        // _shader.SetMatrix4("view", _camera.GetViewMatrix());
        // _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
        //
        // _shader.SetVector3("lightPos", _lightPos);
        // _shader.SetVector3("lightColor", _lightColor);
        // _shader.SetVector3("cameraPos", _camera.Position);
        
        // Передаем параметры камеры в шейдер
        _shader.SetVector3("cameraPos", _camera.Position);
        _shader.SetVector3("cameraFront", -Vector3.Normalize(_camera.Position)); // Направление к центру сцены
        _shader.SetVector3("cameraUp", Vector3.UnitY); // Верх камеры

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

        _movesModule.MouseProcess(MouseState);
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
        // _camera.AspectRatio = Size.X / (float)Size.Y;
    }
}