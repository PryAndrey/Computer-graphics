using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MobiusStrip.Model;

public class MyScene
{
    List<BufferData> _bufferedScene = [];

    readonly List<Vector3> Centres =
    [
        new(0, 0, 0),
        new(0, -0.4f, 0.2f),
        new(0, -1.0f, 0.4f),
        new(-0.06f, 0, -0.2f),
        new(-0.06f, 0, -0.23f),
        new(0.06f, 0, -0.2f),
        new(0.06f, 0, -0.23f),
    ];

    readonly List<float> Sizes =
    [
        0.2f,
        0.3f,
        0.4f,
        0.04f,
        0.02f,
        0.04f,
        0.02f,
    ];

    readonly List<Vector3> Colors =
    [
        new(0, 0, 1),
        new(0, 1, 0),
        new(1, 0, 0),
        new(1, 1, 1),
        new(0, 0, 0),
        new(1, 1, 1),
        new(0, 0, 0),
    ];

    public MyScene()
    {
        _bufferedScene.AddRange(Figure.CreateFloor());
        _bufferedScene.AddRange(GenerateScene());
    }

    public void Draw(Renderer renderer)
    {
        renderer.DrawElements(PrimitiveType.TriangleStrip, Centres, Sizes, _bufferedScene);
    }

    private List<BufferData> GenerateScene()
    {
        List<BufferData> datas = [];

        for (int i = 0; i < Centres.Count; i++)
        {
            // datas.AddRange(Figure.CreateRectangle(Centres[i], Sizes[i], Sizes[i], Sizes[i], Colors[i]));
            // datas.AddRange(Figure.CreateCube(Centres[i], Sizes[i], Colors[i]));
            datas.AddRange(Figure.CreateSphere(Centres[i], Sizes[i], Colors[i]));
        }

        return datas;
    }
    
    public Matrix4 GetModelMatrix()
    {
        return Matrix4.Identity;
    }
}