using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MobiusStrip.Model;

public class MyScene
{
    public static readonly float[] quadVertices =
    {
        -1.0f, -1.0f, 0.0f,
        1.0f, -1.0f, 0.0f,
        -1.0f, 1.0f, 0.0f,
        1.0f, 1.0f, 0.0f
    };

    public void Draw(Renderer renderer)
    {
        renderer.DrawElements();
    }

    public Matrix4 GetModelMatrix()
    {
        return Matrix4.Identity;
    }
}