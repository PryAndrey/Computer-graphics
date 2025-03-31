using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

public class BezierCurve : GameWindow
{
    private Vector2[] controlPoints = new Vector2[]
    {
        new Vector2(-0.5f, -0.5f),
        new Vector2(-0.2f, 0.5f),
        new Vector2(0.2f, 0.5f),
        new Vector2(0.5f, -0.5f)
    };

    private Vector2 selectedPoint;

    public BezierCurve() : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (800, 600) }) { }

    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        // Рисуем кривую Безье
        DrawBezierCurve(controlPoints);

        // Рисуем контрольные точки
        DrawControlPoints(controlPoints);

        SwapBuffers();
    }

    private void DrawBezierCurve(Vector2[] points)
    {
        GL.Begin(PrimitiveType.LineStrip);
        for (float t = 0; t <= 1; t += 0.01f)
        {
            Vector2 point = CalculateBezierPoint(points, t);
            GL.Vertex2(point.X, point.Y);
        }
        GL.End();
    }

    private Vector2 CalculateBezierPoint(Vector2[] points, float t)
    {
        return (1 - t) * (1 - t) * (1 - t) * points[0] +
               3 * (1 - t) * (1 - t) * t * points[1] +
               3 * (1 - t) * t * t * points[2] +
               t * t * t * points[3];
    }

    private void DrawControlPoints(Vector2[] points)
    {
        GL.Color3(1.0f, 1.0f, 1.0f); // Белый цвет
        GL.Begin(PrimitiveType.Points);
        foreach (Vector2 point in points)
        {
            GL.Vertex2(point.X, point.Y);
        }
        GL.End();

        // Рисуем пунктирные линии между контрольными точками
        GL.LineWidth(1);
        GL.LineStipple(1, 0xAAAA); // Пунктирная линия
        GL.Enable(EnableCap.LineStipple);
        GL.Begin(PrimitiveType.LineStrip);
        foreach (Vector2 point in points)
        {
            GL.Vertex2(point.X, point.Y);
        }
        GL.End();
        GL.Disable(EnableCap.LineStipple);
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        base.OnMouseDown(e);
        
        var mouseState = MouseState;
        Vector2 mousePosition = new Vector2(mouseState.X/ (float)Size.X * 2 - 1, 1 - mouseState.Y/ (float)Size.Y * 2);
        float minDistance = float.MaxValue;
        int selectedIndex = -1;

        for (int i = 0; i < controlPoints.Length; i++)
        {
            float distance = Vector2.Distance(mousePosition, controlPoints[i]);
            if (distance < minDistance)
            {
                minDistance = distance;
                selectedIndex = i;
            }
        }

        if (selectedIndex != -1 && minDistance < 0.05f)
        {
            selectedPoint = controlPoints[selectedIndex];
        }
    }

    protected override void OnMouseMove(MouseMoveEventArgs e)
    {
        base.OnMouseMove(e);
        if (selectedPoint != default)
        {
            Vector2 mousePosition = new Vector2(e.Position.X / (float)Size.X * 2 - 1, 1 - e.Position.Y / (float)Size.Y * 2);
            for (int i = 0; i < controlPoints.Length; i++)
            {
                if (controlPoints[i] == selectedPoint)
                {
                    controlPoints[i] = mousePosition;
                    break;
                }
            }
            selectedPoint = default;
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        using (var window = new BezierCurve())
        {
            window.Run();
        }
    }
}
