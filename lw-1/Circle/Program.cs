using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Circle
{
    class Program
    {
        public class Game : GameWindow
        {
            private int _centerX = 0;
            private int _centerY = -1;
            private float _radius = 4;
            private Color4 _circleColor = Color4.Red;
            private Color4 _fillColor = Color4.Blue;
            private bool _fillCircle = true;
            private int _lineWidth = 25;
            private int _scale = 10;

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
                GL.CullFace(TriangleFace.Front);
                // GL.PolygonMode(TriangleFace.Front, PolygonMode.Point);
                // GL.PolygonMode(TriangleFace.Back, PolygonMode.Line);
                GL.Enable(EnableCap.Multisample);
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

                // _radius += 0.09f;
                // if (_radius >= 130)
                // {
                //     _radius = 1;
                // }

                base.OnUpdateFrame(args);
            }

            protected override void OnRenderFrame(FrameEventArgs args)
            {
                GL.Clear(ClearBufferMask.ColorBufferBit);

                List<float> points = CalculateCirclePoints(_centerX, _centerY, (int)_radius);

                if (_fillCircle)
                {
                    DrawFilledCircle(points, _fillColor);
                }

                GL.PointSize(_lineWidth);
                DrawCircle(points, _circleColor);
                SwapBuffers();
                base.OnRenderFrame(args);
            }

            private void DrawVertexArray(int vSize, PrimitiveType type, int verCount, List<float> vertices,
                float[] colors, int start)
            {
                GL.EnableClientState(ArrayCap.VertexArray);

                GL.VertexPointer(vSize, VertexPointerType.Float, 0, vertices.ToArray());

                GL.EnableClientState(ArrayCap.ColorArray);
                GL.ColorPointer(4, ColorPointerType.Float, 0, colors);

                GL.DrawArrays(type, start, verCount - start);

                GL.DisableClientState(ArrayCap.VertexArray);
                GL.DisableClientState(ArrayCap.ColorArray);
            }

            private void DrawCircle(List<float> points, Color4 color)
            {
                int length = points.Count / 3;
                float[] colors = new float[length * 4];

                for (int i = 0; i < length; i++)
                {
                    colors[i * 4] = color.R;
                    colors[i * 4 + 1] = color.G;
                    colors[i * 4 + 2] = color.B;
                    colors[i * 4 + 3] = color.A;
                }

                DrawVertexArray(3, PrimitiveType.Points, length, points, colors, 0);
            }

            private List<float> CalculateCirclePoints(int cx, int cy, int r)
            {
                List<float> points = new List<float>();
                List<float> points1 = new List<float>(),
                    points2 = new List<float>(),
                    points3 = new List<float>(),
                    points4 = new List<float>(),
                    points5 = new List<float>(),
                    points6 = new List<float>(),
                    points7 = new List<float>(),
                    points8 = new List<float>();
                int x = 0;
                int y = r;
                int d = 3 - 2 * r;

                bool isChange = false;
                while (x <= y)
                {
                    if ((isChange || cx + x != cy + y) &&(
                            Math.Abs((float)(cx + x)) / _scale <= 1 + (float)r ||
                            Math.Abs((float)(cy + y)) / _scale <= 1 + (float)r ))
                    {
                        points1.AddRange(new float[] { (float)(cx+x) / _scale, (float)(cy+y) / _scale, 0 });
                        points2.AddRange(new float[] { (float)(cx-x) / _scale, (float)(cy+y) / _scale, 0 });
                        points3.AddRange(new float[] { (float)(cx+x) / _scale, (float)(cy-y) / _scale, 0 });
                        points4.AddRange(new float[] { (float)(cx-x) / _scale, (float)(cy-y) / _scale, 0 });
                        points5.AddRange(new float[] { (float)(cx+y) / _scale, (float)(cy+x) / _scale, 0 });
                        points6.AddRange(new float[] { (float)(cx-y) / _scale, (float)(cy+x) / _scale, 0 });
                        points7.AddRange(new float[] { (float)(cx+y) / _scale, (float)(cy-x) / _scale, 0 });
                        points8.AddRange(new float[] { (float)(cx-y) / _scale, (float)(cy-x) / _scale, 0 });
                    }

                    isChange = false;
                    if (d <= 0)
                    {
                        d = d + 4 * x + 6;
                    }
                    else
                    {
                        d = d + 4 * (x - y) + 10;
                        y--;
                        isChange = true;
                    }

                    x++;
                }

                // 1 5r 7 3r 4 8r 6 2r
                var chunks = points2.Chunk(3);
                List<float> revPoints2 = chunks.Reverse().SelectMany(chunk => chunk).ToList();
                chunks = points3.Chunk(3);
                List<float> revPoints3 = chunks.Reverse().SelectMany(chunk => chunk).ToList();
                chunks = points5.Chunk(3);
                List<float> revPoints5 = chunks.Reverse().SelectMany(chunk => chunk).ToList();
                chunks = points8.Chunk(3);
                List<float> revPoints8 = chunks.Reverse().SelectMany(chunk => chunk).ToList();

                points.AddRange(points1);
                points.AddRange(revPoints5);
                points.AddRange(points7);
                points.AddRange(revPoints3);

                points.AddRange(points4);
                points.AddRange(revPoints8);
                points.AddRange(points6);
                points.AddRange(revPoints2);
                return points;
            }

            private void DrawFilledCircle(List<float> points, Color4 color)
            {
                int length = points.Count / 3;
                float[] colors = new float[length * 4];

                // Заполнение массива значениями цвета
                for (int i = 0; i < length; i++)
                {
                    colors[i * 4] = color.R;
                    colors[i * 4 + 1] = color.G;
                    colors[i * 4 + 2] = color.B;
                    colors[i * 4 + 3] = color.A;
                }

                DrawVertexArray(3, PrimitiveType.TriangleFan, length, points, colors, 0);
            }

            protected override void OnUnload()
            {
                base.OnUnload();
            }
        }

        static void Main()
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
                // NumberOfSamples = 32
            };

            using (Game game = new Game(GameWindowSettings.Default, nativeWinSettings))
            {
                game.Run();
            }
        }
    }
}