using System.Timers;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Task_4.Model;

public class Renderer
{
    private bool _disposed;

    Matrix4 _model = Matrix4.Identity;
    private readonly Shader _shader;
    private readonly Camera _camera;

    private readonly int _vertexArrayObject;
    private readonly int _vertexBufferObject;

    private List<float> _vertices;
    int _vertexCount;
    
    int _textureFrom;
    int _textureTo;

    private readonly int[] _indices;
    
    float _time = 0f;
    Vector2 _clickPosition = new();
    private System.Timers.Timer _timer;

    public Renderer(Shader shader, Camera camera)
    {
        _shader = shader;
        _camera = camera;

        _vertices = GenerateVertices();
        _vertexCount = _vertices.Count;

        _vertexArrayObject = GL.GenVertexArray();
        _vertexBufferObject = GL.GenBuffer();

        GL.BindVertexArray(_vertexArrayObject);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);
        
        GL.BufferData(BufferTarget.ArrayBuffer, _vertexCount * sizeof(float), _vertices.ToArray(), BufferUsageHint.StaticDraw);
        GL.BindVertexArray(0);

        _textureFrom = TextureLoader.Load("C:\\Users\\ANDREY\\Documents\\GitHub\\Computer-graphics\\lw-7\\Task_4\\Model\\Images\\from.jpg");
        _textureTo = TextureLoader.Load("C:\\Users\\ANDREY\\Documents\\GitHub\\Computer-graphics\\lw-7\\Task_4\\Model\\Images\\to.jpg");
        
        _timer = new System.Timers.Timer(16);
        _timer.Elapsed += OnTimerElapsed;
        _timer.AutoReset = true;
    }

    public void OnClick(Vector2 mousePosition)
    {
        _clickPosition.X = mousePosition.X;
        _clickPosition.Y = mousePosition.Y;

        _time = 0f;
        _timer.Start();
    }
    
    List<float> GenerateVertices()
    {
        var vertices = new List<float>
        {

            -1f,  1f, 0f, 0f, 0f,
            -1f, -1f, 0f, 0f, 1f,
            1f, -1f, 0f, 1f, 1f,
            1f,  1f, 0f, 1f, 0f,
        };
        
        return vertices;
    }

    public void DrawElements(bool switchTexture)
    {
        _shader.Use();
        _shader.SetMatrix4("model", _model);
        _shader.SetMatrix4("view", _camera.GetViewMatrix());
        _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
        _shader.SetFloat("Time", _time);
        _shader.SetVector2("ClickPos", _clickPosition);

        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, _textureFrom);
        GL.ActiveTexture(TextureUnit.Texture1);
        GL.BindTexture(TextureTarget.Texture2D, _textureTo);
        if (switchTexture)
        {
            _shader.SetInt("textureTo", 0);
            _shader.SetInt("textureFrom", 1);
            
        }
        else
        {
            _shader.SetInt("textureTo", 1);
            _shader.SetInt("textureFrom", 0);
        }

        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawArrays(PrimitiveType.TriangleFan, 0, _vertexCount / 3);
        GL.BindVertexArray(0);
    }

    public void Dispose()
    {
        if (_disposed) return;
        GL.DeleteVertexArray(_vertexArrayObject);
        GL.DeleteBuffer(_vertexBufferObject);

        _disposed = true;
        GC.SuppressFinalize(this);
    }

    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        _time += 0.02f;

        if (_time > 1.5f)
        {
            _timer.Stop();
        }
    }
    
    ~Renderer()
    {
        Dispose();
    }
}