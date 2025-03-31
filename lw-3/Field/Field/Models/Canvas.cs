namespace Field.Models;

using OpenTK.Graphics.OpenGL;
using System;

public interface ICanvas
{
    void DrawCircle(float x, float y, float rX, float rY, int segments, float r, float g, float b, float a);

    void DrawLine(float x1, float y1, float x2, float y2, float r, float g, float b, float a);
    void DrawRectangle(float x, float y, float width, float height, float r, float g, float b, float a);
    void DrawTriangle(float x1, float y1, float x2, float y2, float x3, float y3, float r, float g, float b, float a);

    void DrawQuad(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4, float r, float g,
        float b, float a);

    void DrawQuadGradient(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4,
        float r1, float g1, float b1, float a1, float r2, float g2, float b2, float a2);

    void ClearScreen(float r, float g, float b, float a);
}

public class Canvas : ICanvas
{
    public void DrawCircle(float x, float y, float rX, float rY, int segments, float r, float g, float b, float a)
    {
        GL.Color4(r, g, b, a);
        GL.Begin(PrimitiveType.TriangleFan);
        GL.Vertex2(x, y); // Центр окружности

        for (int i = 0; i <= segments; i++)
        {
            double angle = 2.0 * Math.PI * i / segments;
            GL.Vertex2(x + Math.Cos(angle) * rX, y + Math.Sin(angle) * rY);
        }

        GL.End();
    }

    public void DrawLine(float x1, float y1, float x2, float y2, float r, float g, float b, float a)
    {
        GL.LineWidth(2);
        GL.Color4(r, g, b, a);
        GL.Begin(PrimitiveType.Lines);
        GL.Vertex2(x1, y1);
        GL.Vertex2(x2, y2);
        GL.End();
        GL.LineWidth(1);
    }

    public void DrawRectangle(float x, float y, float width, float height, float r, float g, float b, float a)
    {
        GL.Color4(r, g, b, a);
        GL.Begin(PrimitiveType.Quads);
        GL.Vertex2(x, y);
        GL.Vertex2(x + width, y);
        GL.Vertex2(x + width, y + height);
        GL.Vertex2(x, y + height);
        GL.End();
    }

    public void DrawQuadGradient(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4,
        float r1, float g1, float b1, float a1, float r2, float g2, float b2, float a2)
    {
        GL.Color4(r1, g1, b1, a1);
        GL.Begin(PrimitiveType.Quads);
        GL.Vertex2(x1, y1);
        GL.Vertex2(x2, y2);
        GL.Color4(r2, g2, b2, a2);
        GL.Vertex2(x3, y3);
        GL.Vertex2(x4, y4);
        GL.End();
    }

    public void DrawQuad(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4, float r,
        float g, float b, float a)
    {
        GL.Color4(r, g, b, a);
        GL.Begin(PrimitiveType.Quads);
        GL.Vertex2(x1, y1);
        GL.Vertex2(x2, y2);
        GL.Vertex2(x3, y3);
        GL.Vertex2(x4, y4);
        GL.End();
    }

    public void DrawTriangle(float x1, float y1, float x2, float y2, float x3, float y3, float r, float g, float b,
        float a)
    {
        GL.Color4(r, g, b, a);
        GL.Begin(PrimitiveType.Triangles);
        GL.Vertex2(x1, y1);
        GL.Vertex2(x2, y2);
        GL.Vertex2(x3, y3);
        GL.End();
    }

    public void ClearScreen(float r, float g, float b, float a)
    {
        GL.ClearColor(r, g, b, a);
    }
}