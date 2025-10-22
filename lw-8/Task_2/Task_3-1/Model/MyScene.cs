using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MobiusStrip.Model;

public class MyScene
{
    List<ParaboloidBufferData> _bufferedScene = [];


    List<ParaboloidData> _paraboloids =
    [
        new(1, 1, new(0, -1, 0)),
        new(0.7f, 0.7f, new(0, 0, 0)),
        new(0.5f, 0.5f, new(0, 0.7f, 0)),
        new(0.3f, 0.3f, new(0, 1.2f, 0)),
    ];

    List<Vector3> _colors =
    [
        new(0.9f, 0.5f, 0.15f),
        new(0.8f, 0.4f, 0.15f),
        new(0.7f, 0.4f, 0.2f),
        new(0.6f, 0.4f, 0.25f),
    ];


    public MyScene()
    {
        GenerateScene();
    }

    public void Draw(Renderer renderer)
    {
        renderer.DrawElements(_paraboloids, _bufferedScene);
    }

    private void GenerateScene()
    {
        int segments = 50;
        int depthStacks = 50;

        for (int i = 0; i < _paraboloids.Count; i++)
        {
            _bufferedScene.Add(Renderer.CreateBufferData(
                Figure.GetParaboloidPoints(_paraboloids[i], _colors[i], segments, depthStacks),
                Figure.GetParaboloidCap(_paraboloids[i], _colors[i], segments))
            );
        }
    }

    public Matrix4 GetModelMatrix()
    {
        return Matrix4.Identity;
    }
}