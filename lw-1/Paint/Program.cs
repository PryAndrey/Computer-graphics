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
            public float MaxHeight;
            public float X { get; set; }
            public float Y { get; set; }

            public float VelocityY { get; set; }

            public float Acceleration { get; set; }

            public Letter(float x, float y, float velocityY, float acceleration, float minHeight, float maxHeight)
            {
                X = x;
                Y = y;
                VelocityY = velocityY;
                Acceleration = acceleration;
                MinHeight = minHeight;
                MaxHeight = maxHeight;
            }

            public void UpdateLetter()
            {
                VelocityY += Acceleration;

                Y += VelocityY;

                // Проверка на достижение максимальной высоты
                if (Y >= MaxHeight)
                {
                    Y = MaxHeight;
                    VelocityY = -VelocityY; // Изменение направления
                }
                // Проверка на достижение минимальной высоты
                else if (Y <= MinHeight)
                {
                    Y = MinHeight;
                    VelocityY = -VelocityY; // Изменение направления
                }
            }
        }

        public class Game : GameWindow
        {
            private Letter _letter1 = new Letter(-0.5f, -0.7f, 0.0f, 0.001f, -0.7f, 0.9f);
            private Letter _letter2 = new Letter(0.0f, -0.7f, 0.0f, 0.002f, -0.7f, 0.9f);
            private Letter _letter3 = new Letter(0.5f, -0.7f, 0.0f, 0.0005f, -0.7f, 0.9f);

            public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
                : base(gameWindowSettings, nativeWindowSettings)
            {
                Console.WriteLine(GL.GetString(StringName.Version));
                VSync = VSyncMode.On;
            }

            protected override void OnLoad()
            {
                base.OnLoad();

                GL.ClearColor(88 / 255.0f, 200 / 255.0f, 248 / 255.0f, 255 / 255.0f);
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
                DrawTruck();
                SwapBuffers();
                base.OnRenderFrame(args);
            }

            protected override void OnUnload()
            {
                base.OnUnload();
            }

            private void DrawTruck()
            {
                // Рисуем дорогу
                {
                    GL.Begin(PrimitiveType.Quads);
                    GL.Color3(0.1f, 0.1f, 0.1f);
                    GL.Vertex2(-1.0f, -0.5f);
                    GL.Vertex2(1.0f, -0.5f);
                    GL.Vertex2(1.0f, -0.35f);
                    GL.Vertex2(-1.0f, -0.35f);
                    GL.End();
                }

                // Рисуем кузов
                {
                    GL.Begin(PrimitiveType.Quads);
                    GL.Color3(0.7f, 0.7f, 0.7f);
                    GL.Vertex2(-0.8f, -0.2f);
                    GL.Vertex2(0.3f, -0.2f);
                    GL.Color3(0.9f, 0.9f, 0.9f);
                    GL.Vertex2(0.3f, 0.7f);
                    GL.Vertex2(-0.8f, 0.7f);
                    GL.Color3(0.2f, 0.2f, 0.2f);
                    GL.Vertex2(-0.8f, -0.2f);
                    GL.Vertex2(0.8f, -0.2f);
                    GL.Vertex2(0.8f, -0.1f);
                    GL.Vertex2(-0.8f, -0.1f);
                    GL.End();
                }

                // Рисуем кабину
                {
                    GL.Color3(0.5f, 0.6f, 0.7f);
                    GL.Begin(PrimitiveType.Quads);
                    GL.Vertex2(0.35f, -0.1f);
                    GL.Vertex2(0.7f, -0.1f);
                    GL.Vertex2(0.7f, 0.45f);
                    GL.Vertex2(0.35f, 0.45f);
                    GL.End();
                    GL.Begin(PrimitiveType.TriangleStrip);
                    GL.Vertex2(0.35f, 0.45f);
                    GL.Vertex2(0.7f, 0.45f);
                    GL.Vertex2(0.35f, 0.5f);
                    GL.Vertex2(0.6f, 0.49f);
                    GL.End();
                    GL.Begin(PrimitiveType.TriangleStrip);
                    GL.Vertex2(0.7f, -0.1f);
                    GL.Vertex2(0.8f, -0.1f);
                    GL.Vertex2(0.7f, 0.2f);
                    GL.Vertex2(0.76f, 0.2f);
                    GL.End();
                }

                // Рисуем окно
                {
                    GL.Color3(0.4f, 0.6f, 1.0f); // Светло-синий цвет
                    GL.Begin(PrimitiveType.Quads);
                    GL.Vertex2(0.4f, 0.25f);
                    GL.Vertex2(0.6f, 0.25f);
                    GL.Vertex2(0.6f, 0.45f);
                    GL.Vertex2(0.4f, 0.45f);
                    GL.End();
                    GL.Begin(PrimitiveType.TriangleStrip);
                    GL.Vertex2(0.6f, 0.25f);
                    GL.Vertex2(0.65f, 0.25f);
                    GL.Vertex2(0.6f, 0.45f);
                    GL.End();
                    GL.Begin(PrimitiveType.TriangleStrip);
                    GL.Vertex2(0.7f, 0.2f);
                    GL.Vertex2(0.75f, 0.2f);
                    GL.Vertex2(0.65f, 0.45f);
                    GL.Vertex2(0.7f, 0.45f);
                    GL.End();
                }

                // Рисуем дверь
                {
                    GL.LineWidth(2);
                    GL.Color3(0.0f, 0.0f, 0.0f);
                    GL.Begin(PrimitiveType.Lines);
                    GL.Vertex2(0.4f, 0.25f);
                    GL.Vertex2(0.4f, -0.05f);
                    GL.Vertex2(0.4f, -0.05f);
                    GL.Vertex2(0.7f, -0.05f);
                    GL.Vertex2(0.7f, -0.05f);
                    GL.Vertex2(0.65f, 0.25f);
                    GL.End();
                    GL.Begin(PrimitiveType.Quads);
                    GL.Color3(0.3f, 0.3f, 0.3f);
                    GL.Vertex2(0.42f, 0.2f);
                    GL.Vertex2(0.5f, 0.2f);
                    GL.Vertex2(0.5f, 0.22f);
                    GL.Vertex2(0.42f, 0.22f);
                    GL.End();
                }

                // Рисуем колеса
                {
                    GL.Color3(0.0f, 0.0f, 0.0f); // Черный цвет

                    DrawCircle(-0.5f, -0.3f, 0.2f);
                    GL.Color3(0.4f, 0.4f, 0.4f);
                    DrawCircle(-0.5f, -0.3f, 0.15f);
                    GL.Color3(0.0f, 0.0f, 0.0f); // Черный цвет
                    DrawCircle(0.5f, -0.3f, 0.2f);
                    GL.Color3(0.4f, 0.4f, 0.4f);
                    DrawCircle(0.5f, -0.3f, 0.15f);
                }

                // Рисуем фары
                {
                    GL.Color3(1.0f, 1.0f, 0.0f); // Черный цвет
                    DrawCircle(0.75f, -0.07f, 0.02f);
                }
            }

            private void DrawCircle(float centerX, float centerY, float radius)
            {
                GL.Begin(PrimitiveType.TriangleFan);
                GL.Vertex2(centerX, centerY); // Центр колеса
                for (int i = 0; i <= 100; i++)
                {
                    double angle = 2.0 * Math.PI * i / 100;
                    double x = centerX + (radius * Math.Cos(angle));
                    double y = centerY + (radius * Math.Sin(angle));
                    GL.Vertex2(x, y);
                }

                GL.End();
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