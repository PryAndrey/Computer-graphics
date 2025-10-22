using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;


public class MyScene
{
    List<BufferData> _bufferedScene = [];

    List<TorusData> _toruses =
    [
        new(new Vector3(0, -0.8f, 0), 0.9f, 0.3f),
        new(new Vector3(0, -0.3f, 0), 0.7f, 0.25f),
        new(new Vector3(0, 0.1f, 0), 0.5f, 0.2f),
        new(new Vector3(0, 0.4f, 0), 0.3f, 0.15f),
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
        renderer.DrawElements(_toruses, _bufferedScene);
    }

    private void GenerateScene()
    {
        int segments = 50;
        int crossSectionSegments = 30;

        for (int i = 0; i < _toruses.Count; i++)
        {
            _bufferedScene.Add(
                Renderer.CreateBufferData(Figure.CreateTorusPoints(_toruses[i], _colors[i], segments, crossSectionSegments)));
        }
    }

    public Matrix4 GetModelMatrix()
    {
        return Matrix4.Identity;
    }
}