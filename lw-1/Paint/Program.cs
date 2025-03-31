using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Test
{
    class Program
    {
        public class Truck
        {
            public float X { get; set; } = 0.0f;
            public float Y { get; set; } = 0.0f;

            public (float[] vertices, float[] colors, PrimitiveType type) GetRoadData()
            {
                float[] vertices = new[]
                {
                    -1.0f + X, -0.5f + Y, 0.0f,
                    1.0f + X, -0.5f + Y, 0.0f,
                    1.0f + X, -0.35f + Y, 0.0f,
                    -1.0f + X, -0.35f + Y, 0.0f
                };

                float[] colors = new[]
                {
                    0.1f, 0.1f, 0.1f, 1.0f,
                    0.1f, 0.1f, 0.1f, 1.0f,
                    0.1f, 0.1f, 0.1f, 1.0f,
                    0.1f, 0.1f, 0.1f, 1.0f
                };

                return (vertices, colors, PrimitiveType.TriangleFan);
            }

            public (float[] vertices, float[] colors, PrimitiveType type)[] GetBodyData()
            {
                return new (float[], float[], PrimitiveType type)[]
                {
                    (
                        new float[]
                        {
                            -0.8f + X, -0.2f + Y, 0.0f,
                            0.3f + X, -0.2f + Y, 0.0f,
                            0.3f + X, 0.7f + Y, 0.0f,
                            -0.8f + X, 0.7f + Y, 0.0f
                        },
                        new float[]
                        {
                            0.7f, 0.7f, 0.7f, 1.0f,
                            0.7f, 0.7f, 0.7f, 1.0f,
                            0.9f, 0.9f, 0.9f, 1.0f,
                            0.9f, 0.9f, 0.9f, 1.0f
                        },
                        PrimitiveType.TriangleFan
                    ),
                    (
                        new float[]
                        {
                            -0.8f + X, -0.2f + Y, 0.0f,
                            0.8f + X, -0.2f + Y, 0.0f,
                            0.8f + X, -0.1f + Y, 0.0f,
                            -0.8f + X, -0.1f + Y, 0.0f
                        },
                        new float[]
                        {
                            0.2f, 0.2f, 0.2f, 1.0f,
                            0.2f, 0.2f, 0.2f, 1.0f,
                            0.2f, 0.2f, 0.2f, 1.0f,
                            0.2f, 0.2f, 0.2f, 1.0f
                        },
                        PrimitiveType.TriangleFan
                    )
                };
            }

            public (float[] vertices, float[] colors, PrimitiveType type)[] GetCabData()
            {
                return new (float[], float[], PrimitiveType type)[]
                {
                    (
                        new float[]
                        {
                            0.35f + X, -0.1f + Y, 0.0f,
                            0.7f + X, -0.1f + Y, 0.0f,
                            0.7f + X, 0.45f + Y, 0.0f,
                            0.35f + X, 0.45f + Y, 0.0f
                        },
                        new float[]
                        {
                            0.5f, 0.6f, 0.7f, 1.0f,
                            0.5f, 0.6f, 0.7f, 1.0f,
                            0.5f, 0.6f, 0.7f, 1.0f,
                            0.5f, 0.6f, 0.7f, 1.0f
                        },
                        PrimitiveType.TriangleFan
                    ),
                    (
                        new float[]
                        {
                            0.35f + X, 0.45f + Y, 0.0f,
                            0.7f + X, 0.45f + Y, 0.0f,
                            0.35f + X, 0.5f + Y, 0.0f,
                            0.6f + X, 0.49f + Y, 0.0f
                        },
                        new float[]
                        {
                            0.5f, 0.6f, 0.7f, 1.0f,
                            0.5f, 0.6f, 0.7f, 1.0f,
                            0.5f, 0.6f, 0.7f, 1.0f,
                            0.5f, 0.6f, 0.7f, 1.0f
                        },
                        PrimitiveType.TriangleStrip
                    ),
                    (
                        new float[]
                        {
                            0.7f + X, -0.1f + Y, 0.0f,
                            0.8f + X, -0.1f + Y, 0.0f,
                            0.7f + X, 0.2f + Y, 0.0f,
                            0.76f + X, 0.2f + Y, 0.0f
                        },
                        new float[]
                        {
                            0.5f, 0.6f, 0.7f, 1.0f,
                            0.5f, 0.6f, 0.7f, 1.0f,
                            0.5f, 0.6f, 0.7f, 1.0f,
                            0.5f, 0.6f, 0.7f, 1.0f
                        },
                        PrimitiveType.TriangleStrip
                    )
                };
            }

            public (float[] vertices, float[] colors, PrimitiveType type)[] GetWindowData()
            {
                return new (float[], float[], PrimitiveType type)[]
                {
                    (
                        new float[]
                        {
                            0.4f + X, 0.25f + Y, 0.0f,
                            0.6f + X, 0.25f + Y, 0.0f,
                            0.6f + X, 0.45f + Y, 0.0f,
                            0.4f + X, 0.45f + Y, 0.0f
                        },
                        new float[]
                        {
                            0.4f, 0.6f, 1.0f, 1.0f,
                            0.4f, 0.6f, 1.0f, 1.0f,
                            0.4f, 0.6f, 1.0f, 1.0f,
                            0.4f, 0.6f, 1.0f, 1.0f
                        },
                        PrimitiveType.TriangleFan
                    ),
                    (
                        new float[]
                        {
                            0.6f + X, 0.25f + Y, 0.0f,
                            0.65f + X, 0.25f + Y, 0.0f,
                            0.6f + X, 0.45f + Y, 0.0f
                        },
                        new float[]
                        {
                            0.4f, 0.6f, 1.0f, 1.0f,
                            0.4f, 0.6f, 1.0f, 1.0f,
                            0.4f, 0.6f, 1.0f, 1.0f
                        },
                        PrimitiveType.TriangleStrip
                    ),
                    (
                        new float[]
                        {
                            0.7f + X, 0.2f + Y, 0.0f,
                            0.75f + X, 0.2f + Y, 0.0f,
                            0.65f + X, 0.45f + Y, 0.0f,
                            0.7f + X, 0.45f + Y, 0.0f
                        },
                        new float[]
                        {
                            0.4f, 0.6f, 1.0f, 1.0f,
                            0.4f, 0.6f, 1.0f, 1.0f,
                            0.4f, 0.6f, 1.0f, 1.0f,
                            0.4f, 0.6f, 1.0f, 1.0f
                        },
                        PrimitiveType.TriangleStrip
                    )
                };
            }

            public (float[] vertices, float[] colors, PrimitiveType type)[] GetDoorData()
            {
                return new (float[], float[], PrimitiveType type)[]
                {
                    (
                        new float[]
                        {
                            0.4f + X, 0.25f + Y, 0.0f,
                            0.4f + X, -0.05f + Y, 0.0f,
                            0.4f + X, -0.05f + Y, 0.0f,
                            0.7f + X, -0.05f + Y, 0.0f,
                            0.7f + X, -0.05f + Y, 0.0f,
                            0.65f + X, 0.25f + Y, 0.0f
                        },
                        new float[]
                        {
                            0.0f, 0.0f, 0.0f, 1.0f,
                            0.0f, 0.0f, 0.0f, 1.0f,
                            0.0f, 0.0f, 0.0f, 1.0f,
                            0.0f, 0.0f, 0.0f, 1.0f,
                            0.0f, 0.0f, 0.0f, 1.0f,
                            0.0f, 0.0f, 0.0f, 1.0f
                        },
                        PrimitiveType.Lines
                    ),
                    (
                        new float[]
                        {
                            0.42f + X, 0.2f + Y, 0.0f,
                            0.5f + X, 0.2f + Y, 0.0f,
                            0.5f + X, 0.22f + Y, 0.0f,
                            0.42f + X, 0.22f + Y, 0.0f
                        },
                        new float[]
                        {
                            0.3f, 0.3f, 0.3f, 1.0f,
                            0.3f, 0.3f, 0.3f, 1.0f,
                            0.3f, 0.3f, 0.3f, 1.0f,
                            0.3f, 0.3f, 0.3f, 1.0f
                        },
                        PrimitiveType.TriangleFan
                    )
                };
            }

            public (float x, float y, float radius, float[] colors)[] GetWheelData()
            {
                return new (float x, float y, float radius, float[] colors)[]
                {
                    (-0.5f + X, -0.3f + Y, 0.2f, new float[] { 0.0f, 0.0f, 0.0f }),
                    (-0.5f + X, -0.3f + Y, 0.15f, new float[] { 0.4f, 0.4f, 0.4f }),
                    (0.5f + X, -0.3f + Y, 0.2f, new float[] { 0.0f, 0.0f, 0.0f }),
                    (0.5f + X, -0.3f + Y, 0.15f, new float[] { 0.4f, 0.4f, 0.4f }),
                };
            }

            public (float x, float y, float radius, float[] colors) GetLightData()
            {
                return new(0.75f + X, -0.07f + Y, 0.02f, new float[] { 1.0f, 1.0f, 0.0f });
            }
        }

        public class Game : GameWindow
        {
            private Truck _truck = new Truck();
            private Vector2 _mousePosition = Vector2.Zero;
            private bool _isDragging = false;
            private Vector2 _dragOffset;

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

            protected override void OnMouseDown(MouseButtonEventArgs e)
            {
                base.OnMouseDown(e);

                if (e.Button == MouseButton.Left)
                {
                    var mouseState = MouseState;
                    Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
                    Vector2 worldPosition = ScreenToWorld(mousePosition);
                    _isDragging = true;
                    _dragOffset = worldPosition - this._mousePosition;
                }
            }

            protected override void OnMouseUp(MouseButtonEventArgs e)
            {
                base.OnMouseUp(e);

                if (e.Button == MouseButton.Left)
                {
                    _isDragging = false;
                }
            }

            protected override void OnMouseMove(MouseMoveEventArgs e)
            {
                base.OnMouseMove(e);

                if (_isDragging)
                {
                    Vector2 mousePosition = new Vector2(e.X, e.Y);
                    Vector2 worldPosition = ScreenToWorld(mousePosition);
                    this._mousePosition = worldPosition - _dragOffset;
                }
            }

            private Vector2 ScreenToWorld(Vector2 screenPosition)
            {
                float x = (screenPosition.X / ClientSize.X) * 2.0f - 1.0f;
                float y = 1.0f - (screenPosition.Y / ClientSize.Y) * 2.0f;
                return new Vector2(x, y);
            }

            private void DrawVertexArray(int vSize, PrimitiveType type, int verCount, float[] vertices, float[] colors)
            {
                GL.EnableClientState(ArrayCap.VertexArray);
                GL.VertexPointer(vSize, VertexPointerType.Float, 0, vertices);

                GL.EnableClientState(ArrayCap.ColorArray);
                GL.ColorPointer(4, ColorPointerType.Float, 0, colors);

                GL.DrawArrays(type, 0, verCount);

                GL.DisableClientState(ArrayCap.VertexArray);
                GL.DisableClientState(ArrayCap.ColorArray);
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

                base.OnUpdateFrame(args);
            }
// todo отрисовка при ресайзе
            protected override void OnRenderFrame(FrameEventArgs args)
            {
                GL.Clear(ClearBufferMask.ColorBufferBit);

                GL.PushMatrix();
                GL.Translate(_mousePosition.X, _mousePosition.Y, 0.0f);

                DrawTruck();

                GL.PopMatrix();

                SwapBuffers();
                base.OnRenderFrame(args);
            }

            protected override void OnUnload()
            {
                base.OnUnload();
            }

            // todo вынести функцию отрисовки
            private void DrawTruck()
            {
                // Рисуем дорогу
                {
                    var (roadVertices, roadColors, type) = _truck.GetRoadData();
                    DrawVertexArray(3, type, 4, roadVertices, roadColors);
                }

                // Рисуем кузов
                foreach (var (vertices, colors, type) in _truck.GetBodyData())
                {
                    DrawVertexArray(3, type, 4, vertices, colors);
                }

                // Рисуем кабину
                foreach (var (vertices, colors, type) in _truck.GetCabData())
                {
                    DrawVertexArray(3, type, 4, vertices, colors);
                }

                // Рисуем окно
                foreach (var (vertices, colors, type) in _truck.GetWindowData())
                {
                    DrawVertexArray(3, type, 4, vertices, colors);
                }

                // Рисуем дверь
                GL.LineWidth(2);
                foreach (var (vertices, colors, type) in _truck.GetDoorData())
                {
                    DrawVertexArray(3, type, vertices.Length / 3, vertices, colors);
                }

                // Рисуем колеса
                {
                    foreach (var (x, y, r, color) in _truck.GetWheelData())
                    {
                        GL.Color3(color[0], color[1], color[2]);
                        DrawCircle(x, y, r);
                    }
                }

                // Рисуем фары
                {
                    var (x, y, r, color) = _truck.GetLightData();
                    GL.Color3(color[0], color[1], color[2]);
                    DrawCircle(x, y, r);
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