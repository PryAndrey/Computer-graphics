using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Test
{
    class Program
    {
        struct Letter
        {
            public float MinHeight;
            public float X { get; set; }
            public float Y { get; set; }

            public float VelocityY { get; set; }
            public float MaxVelocityY { get; set; }

            public float Acceleration { get; set; }

            public Letter(float x, float y, float velocityY, float acceleration, float minHeight)
            {
                X = x;
                Y = y;
                VelocityY = velocityY;
                MaxVelocityY = velocityY;
                Acceleration = acceleration;
                MinHeight = minHeight;
            }

            public void UpdateLetter()
            {
                VelocityY -= Acceleration;

                Y += VelocityY;

                if (Y <= MinHeight)
                {
                    Y = MinHeight;
                    VelocityY = MaxVelocityY;
                }
            }
        }

        public class Game : GameWindow
        {
            private Letter _letter1 = new Letter(-0.5f, -0.7f, 0.05f, 0.001f, -0.7f);
            private Letter _letter2 = new Letter(0.0f, -0.7f, 0.05f, 0.002f, -0.7f);
            private Letter _letter3 = new Letter(0.5f, -0.7f, 0.05f, 0.0015f, -0.7f);

            public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
                : base(gameWindowSettings, nativeWindowSettings)
            {
                Console.WriteLine(GL.GetString(StringName.Version));
                VSync = VSyncMode.On;
            }

            protected override void OnLoad()
            {
                base.OnLoad();

                GL.ClearColor(0 / 255.0f, 0 / 255.0f, 0 / 255.0f, 255 / 255.0f);
                GL.Enable(EnableCap.CullFace);
                GL.CullFace(TriangleFace.Back);
                // GL.PolygonMode(TriangleFace.Front, PolygonMode.Point);
                // GL.PolygonMode(TriangleFace.Front, PolygonMode.Line);
            }

            protected override void OnResize(ResizeEventArgs e)
            {
                base.OnResize(e);
            }

            protected override void OnUpdateFrame(FrameEventArgs args)
            {
                var key = KeyboardState;

                if (key.IsKeyDown(Keys.Escape))
                {
                    Console.WriteLine(Keys.Escape.ToString());
                    Close();
                }

                _letter1.UpdateLetter();
                _letter2.UpdateLetter();
                _letter3.UpdateLetter();

                base.OnUpdateFrame(args);
            }

            protected override void OnRenderFrame(FrameEventArgs args)
            {
                GL.Clear(ClearBufferMask.ColorBufferBit);
                DrawLetters();
                SwapBuffers();
                base.OnRenderFrame(args);
            }

            protected override void OnUnload()
            {
                base.OnUnload();
            }

            private void DrawA(float xDiff, float yDiff, float width, float height)
            {
                // Рисуем левую часть буквы "А"
                GL.Begin(PrimitiveType.TriangleStrip);
                GL.Vertex2(-0.5f * width + xDiff, -1.0f * height + yDiff); // Нижняя левая
                GL.Vertex2(-0.3f * width + xDiff, -1.0f * height + yDiff); // Нижняя правая
                GL.Vertex2(0.0f * width + xDiff, 0.7f * height + yDiff); // Верхняя левая
                GL.Vertex2(0.0f * width + xDiff, 0.0f * height + yDiff); // Верхняя правая
                GL.End();

                // Рисуем правую часть буквы "А"
                GL.Begin(PrimitiveType.TriangleStrip);
                GL.Vertex2(0.3f * width + xDiff, -1.0f * height + yDiff); // Нижняя правая
                GL.Vertex2(0.5f * width + xDiff, -1.0f * height + yDiff); // Нижняя левая
                GL.Vertex2(0.0f * width + xDiff, 0.0f * height + yDiff); // Верхняя правая
                GL.Vertex2(0.0f * width + xDiff, 0.7f * height + yDiff); // Верхняя левая
                GL.End();

                // Рисуем перекладину буквы "А"
                GL.Begin(PrimitiveType.TriangleStrip);
                GL.Vertex2(-0.4f * width + xDiff, -0.7f * height + yDiff); // Левая нижняя
                GL.Vertex2(0.4f * width + xDiff, -0.7f * height + yDiff); // Правая нижняя
                GL.Vertex2(-0.3f * width + xDiff, -0.4f * height + yDiff); // Левая верхняя
                GL.Vertex2(0.3f * width + xDiff, -0.4f * height + yDiff); // Правая верхняя
                GL.End();
            }

            private void DrawP(float xDiff, float yDiff, float width, float height)
            {
                // Рисуем левую часть буквы "П"
                GL.Begin(PrimitiveType.TriangleStrip);
                GL.Vertex2(-0.5f * width + xDiff, -1.0f * height + yDiff); // Нижняя левая
                GL.Vertex2(-0.3f * width + xDiff, -1.0f * height + yDiff); // Нижняя правая
                GL.Vertex2(-0.5f * width + xDiff, 0.7f * height + yDiff); // Верхняя левая
                GL.Vertex2(-0.3f * width + xDiff, 0.7f * height + yDiff); // Верхняя правая
                GL.End();

                // Рисуем правую часть буквы "П"
                GL.Begin(PrimitiveType.TriangleStrip);
                GL.Vertex2(0.3f * width + xDiff, -1.0f * height + yDiff); // Нижняя правая
                GL.Vertex2(0.5f * width + xDiff, -1.0f * height + yDiff); // Нижняя левая
                GL.Vertex2(0.3f * width + xDiff, 0.7f * height + yDiff); // Верхняя правая
                GL.Vertex2(0.5f * width + xDiff, 0.7f * height + yDiff); // Верхняя левая
                GL.End();

                // Рисуем перекладину буквы "П"
                GL.Begin(PrimitiveType.TriangleStrip);
                GL.Vertex2(-0.3f * width + xDiff, 0.5f * height + yDiff); // Левая нижняя
                GL.Vertex2(0.3f * width + xDiff, 0.5f * height + yDiff); // Правая нижняя
                GL.Vertex2(-0.3f * width + xDiff, 0.7f * height + yDiff); // Левая верхняя
                GL.Vertex2(0.3f * width + xDiff, 0.7f * height + yDiff); // Правая верхняя
                GL.End();
            }

            private void DrawLetters()
            {
                GL.Color3(1.0f, 0.0f, 0.0f);
                DrawA(-0.5f, _letter1.Y, 0.2f, 0.2f);
                GL.Color3(0.0f, 1.0f, 0.0f);
                DrawA(0.0f, _letter2.Y, 0.2f, 0.2f);
                GL.Color3(0.0f, 0.0f, 1.0f);
                DrawP(0.5f, _letter3.Y, 0.2f, 0.2f);
            }
        }

        static void Main(string[] args)
        {
            var nativeWinSettings = new NativeWindowSettings()
            {
                ClientSize = new Vector2i(600, 600),
                Location = new Vector2i(370, 300),
                WindowBorder = WindowBorder.Resizable,
                WindowState = WindowState.Normal,
                Title = "LearnOpenTK - Creating a Window",

                Flags = ContextFlags.Default,
                APIVersion = new Version(4, 6),
                Profile = ContextProfile.Compatability,
                API = ContextAPI.OpenGL,

                NumberOfSamples = 0
            };

            using (Game game = new Game(GameWindowSettings.Default, nativeWinSettings))
            {
                game.Run();
            }
        }
    }
}