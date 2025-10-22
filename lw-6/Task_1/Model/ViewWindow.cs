using System.Drawing;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;


    public class ViewWindow : GameWindow
    {
        private MyScene _myScene;
        private Camera _camera;
        private MovesModule _movesModule;

        public ViewWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(Color.White);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.Normalize);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Enable(EnableCap.Texture2D);

            SetLight();
            
            _myScene = new MyScene();
            _camera = new Camera(Size.X / (float)Size.Y, (float)Math.PI, (float)Math.PI);
            _movesModule = new MovesModule(_camera);
            
            CursorState = CursorState.Grabbed;
        }

        private void SetLight()
        {
            Vector4 lightPosition = new Vector4(0f, 10f, 1f, 1f);
            Vector4 lightDiffuse = new Vector4(1f, 1f, 1f, 1f);
            Vector4 lightAmbient = new Vector4(0.3f, 0.3f, 0.3f, 1f);
            Vector4 lightSpecular = new Vector4(1f, 1f, 1f, 1f);

            GL.Light(LightName.Light0, LightParameter.Position, lightPosition);
            GL.Light(LightName.Light0, LightParameter.Diffuse, lightDiffuse);
            GL.Light(LightName.Light0, LightParameter.Ambient, lightAmbient);
            GL.Light(LightName.Light0, LightParameter.Specular, lightSpecular);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            
            if (KeyboardState.IsKeyDown(Keys.Escape)) Close();
            _movesModule.MoveProcess(KeyboardState);
            _movesModule.MouseProcess(MouseState);
            
            _myScene.Update((float)args.Time);
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
            
            Matrix4 projection = _camera.GetProjectionMatrix();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            GL.MatrixMode(MatrixMode.Modelview);
            
            var view = _camera.GetViewMatrix();
            GL.LoadMatrix(ref view);
            
            _myScene.Draw();
            
            SwapBuffers();
        }
    }
