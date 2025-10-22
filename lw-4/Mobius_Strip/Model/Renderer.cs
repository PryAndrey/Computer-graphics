using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace MobiusStrip.Utilities;

public class Renderer
{
    private bool _disposed;
    private readonly int _vertexArrayObject;
    private readonly int _vertexBufferObject;
    private RotationCamera _rotationCamera;

    public Renderer(RotationCamera camera)
    {
        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, 0, IntPtr.Zero, BufferUsageHint.DynamicDraw);

        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);

        _rotationCamera = camera;

        GL.EnableClientState(ArrayCap.VertexArray);
        GL.EnableClientState(ArrayCap.ColorArray);

        GL.VertexPointer(3, VertexPointerType.Float, VertexElement.Size * sizeof(float), 0);

        GL.ColorPointer(3, ColorPointerType.Float, VertexElement.Size * sizeof(float),
            VertexElement.ColorIndex * sizeof(float));

        GL.Enable(EnableCap.Lighting);
        GL.Enable(EnableCap.Light0);

        Vector4 lightPos = new Vector4(1, 1, 1, 0);
        GL.Light(LightName.Light0, LightParameter.Position,
            new float[] { lightPos.X, lightPos.Y, lightPos.Z, lightPos.W });
        GL.Light(LightName.Light0, LightParameter.Diffuse, new float[] { 1f, 1f, 1f, 1f });
        GL.Light(LightName.Light0, LightParameter.Ambient, new float[] { 0.2f, 0.2f, 0.2f, 1f });

        GL.Enable(EnableCap.ColorMaterial);
        GL.ColorMaterial(MaterialFace.FrontAndBack, ColorMaterialParameter.AmbientAndDiffuse);
    }

    public void DrawElements(PrimitiveType primitiveType, List<VertexElement> verticesList, Vector3 modelMatrixPosition)
    {
        GL.PushMatrix();

        // 1. Настройка матриц проекции и вида
        GL.MatrixMode(MatrixMode.Projection);
        var projectionMatrix = _rotationCamera.GetProjectionMatrix();
        GL.LoadMatrix(ref projectionMatrix);

        GL.MatrixMode(MatrixMode.Modelview);
        var viewMatrix = _rotationCamera.GetViewMatrix();
        GL.LoadMatrix(ref viewMatrix);

        // 2. Применяем трансформации модели
        GL.Translate(modelMatrixPosition);

        // 3. Настройка освещения (должна выполняться один раз при инициализации)
        GL.Enable(EnableCap.Lighting);
        GL.Enable(EnableCap.Light0);

        // Позиция света в мировых координатах (направленный свет сверху-справа-спереди)
        Vector4 lightPosition = new Vector4(1f, 1f, 1f, 0f); // w=0 для направленного света
        GL.Light(LightName.Light0, LightParameter.Position,
            new float[] { lightPosition.X, lightPosition.Y, lightPosition.Z, lightPosition.W });

        // Параметры света
        GL.Light(LightName.Light0, LightParameter.Diffuse, new float[] { 1f, 1f, 1f, 1f }); // Белый диффузный свет
        GL.Light(LightName.Light0, LightParameter.Ambient, new float[] { 0.2f, 0.2f, 0.2f, 1f }); // Слабый фоновый свет
        GL.Light(LightName.Light0, LightParameter.Specular, new float[] { 1f, 1f, 1f, 1f }); // Зеркальные блики

        // 4. Настройка материала
        GL.Enable(EnableCap.ColorMaterial);
        GL.ColorMaterial(MaterialFace.FrontAndBack, ColorMaterialParameter.AmbientAndDiffuse);

        // Дополнительные параметры материала (опционально)
        GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Shininess, 32f); // Блеск материала

        // 5. Активация нормалей (критически важно!)
        GL.EnableClientState(ArrayCap.NormalArray);
        // Указываем, где брать нормали (предполагаем, что они идут после позиции в VertexElement)
        GL.NormalPointer(NormalPointerType.Float, VertexElement.Size * sizeof(float),
            (IntPtr)(VertexElement.PositionIndex * sizeof(float)));

        // 6. Обновление и отрисовка буферов
        var verticesArray = verticesList.SelectMany(v => v.ToArray()).ToArray();
        UpdateBuffers(verticesArray);

        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawArrays(primitiveType, 0, verticesArray.Length / VertexElement.Size);

        // 7. Восстановление состояния
        GL.DisableClientState(ArrayCap.NormalArray);
        GL.PopMatrix();
    }

    private void UpdateBuffers(float[] vertices)
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.DynamicDraw);
    }// todo разрезать 2 раза вдоль ленту мебиуса из бумаги

    public void Dispose()
    {
        if (_disposed) return;

        GL.DeleteVertexArray(_vertexArrayObject);
        GL.DeleteBuffer(_vertexBufferObject);

        _disposed = true;
        GC.SuppressFinalize(this);
    }

    ~Renderer()
    {
        Dispose();
    }
}