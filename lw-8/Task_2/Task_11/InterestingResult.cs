using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

/*
class Program
{
    static void Main()
    {
        var settings = GameWindowSettings.Default;
        var nativeSettings = new NativeWindowSettings()
        {
            Size = new Vector2i(800, 600),
            Title = "Raymarching with Reflections",
            Flags = ContextFlags.ForwardCompatible
        };

        using var window = new RaymarchingWindow(settings, nativeSettings);
        window.Run();
    }
}*/

public class RaymarchingWindow : GameWindow
{
    private int _shaderProgram;
    private int _vertexArrayObject;
    private double _time;
    
    public RaymarchingWindow(GameWindowSettings gameSettings, NativeWindowSettings nativeSettings)
        : base(gameSettings, nativeSettings)
    {
    }
    
    protected override void OnLoad()
    {
        base.OnLoad();
        
        // Инициализация OpenGL
        GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
        
        // Создаем шейдерную программу
        _shaderProgram = CreateShaderProgram();
        
        // Создаем VAO для полноэкранного квада
        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);
        
        // Простой квад, покрывающий весь экран
        float[] quadVertices = {
            -1.0f, -1.0f, 0.0f,
             1.0f, -1.0f, 0.0f,
            -1.0f,  1.0f, 0.0f,
             1.0f,  1.0f, 0.0f
        };
        
        int vertexBuffer = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, quadVertices.Length * sizeof(float), quadVertices, BufferUsageHint.StaticDraw);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
        GL.EnableVertexAttribArray(0);
    }
    
    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        
        _time += args.Time;
        
        // Активируем шейдер
        GL.UseProgram(_shaderProgram);
        
        // Передаем время в шейдер
        GL.Uniform1(GL.GetUniformLocation(_shaderProgram, "time"), (float)_time);
        
        // Передаем разрешение экрана
        GL.Uniform2(GL.GetUniformLocation(_shaderProgram, "resolution"), new Vector2(Size.X, Size.Y));
        
        // Рисуем квад
        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
        
        SwapBuffers();
    }
    
    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, Size.X, Size.Y);
    }
    
private int CreateShaderProgram()
{
    // Вершинный шейдер (оставляем без изменений)
    string vertexShaderSource = @"
#version 330 core
layout (location = 0) in vec3 aPos;
void main()
{
    gl_Position = vec4(aPos, 1.0);
}
    ";
    
    // Фрагментный шейдер с отладочной информацией
    string fragmentShaderSource = @"
#version 330 core
out vec4 FragColor;

uniform float time;
uniform vec2 resolution;

struct HitInfo {
    float distance;
    vec3 position;
    vec3 normal;
    int material;
};

#define MATERIAL_GROUND 0
#define MATERIAL_SPHERE 1
#define MATERIAL_METAL 2

float sdSphere(vec3 p, float r) {
    return length(p) - r;
}

float sdPlane(vec3 p, vec3 n, float h) {
    return dot(p, n) + h;
}

HitInfo scene(vec3 p) {
    HitInfo hit;
    hit.distance = 1e10;
    hit.material = MATERIAL_GROUND;
    
    
    float ground = sdPlane(p, vec3(0, 1, 0), 0.0);
    if (ground < hit.distance) {
        hit.distance = ground;
        hit.material = MATERIAL_GROUND;
    }
    
    
    vec3 spherePos = vec3(sin(time * 0.5) * 2.0, 0.5, -3.0);
    float sphere = sdSphere(p - spherePos, 0.8);
    if (sphere < hit.distance) {
        hit.distance = sphere;
        hit.material = MATERIAL_SPHERE;
    }
    
    
    vec3 cubePos = vec3(0.0, 0.5, -5.0);
    vec3 q = abs(p - cubePos) - vec3(0.5);
    float cube = length(max(q, 0.0)) + min(max(q.x, max(q.y, q.z)), 0.0);
    if (cube < hit.distance) {
        hit.distance = cube;
        hit.material = MATERIAL_METAL;
    }
    
    return hit;
}

vec3 calcNormal(vec3 p) {
    const float eps = 0.001;
    vec2 e = vec2(eps, 0);
    return normalize(vec3(
        scene(p + e.xyy).distance - scene(p - e.xyy).distance,
        scene(p + e.yxy).distance - scene(p - e.yxy).distance,
        scene(p + e.yyx).distance - scene(p - e.yyx).distance
    ));
}

HitInfo raymarch(vec3 ro, vec3 rd) {
    HitInfo hit;
    hit.distance = 0.0;
    
    for (int i = 0; i < 100; i++) {
        vec3 p = ro + rd * hit.distance;
        HitInfo info = scene(p);
        
        if (info.distance < 0.001) {
            hit.position = p;
            hit.normal = calcNormal(p);
            hit.material = info.material;
            hit.distance += info.distance;
            return hit;
        }
        
        if (hit.distance > 100.0) break;
        
        hit.distance += info.distance;
    }
    
    hit.distance = 1e10;
    return hit;
}

vec3 getMaterialColor(int material, vec3 pos) {
    if (material == MATERIAL_GROUND) {
        
        float tile = mod(floor(pos.x) + floor(pos.z), 2.0);
        return mix(vec3(0.8), vec3(0.3), tile);
    } else if (material == MATERIAL_SPHERE) {
        return vec3(0.2, 0.5, 0.8);
    } else if (material == MATERIAL_METAL) {
        return vec3(0.8, 0.8, 0.9);
    }
    return vec3(1.0, 0.0, 1.0); 
}

void main() {
    vec2 uv = (gl_FragCoord.xy - 0.5 * resolution.xy) / resolution.y;
    
    
    vec3 ro = vec3(0.0, 1.0, 3.0); 
    vec3 rd = normalize(vec3(uv, -1.0)); 
    
    HitInfo hit = raymarch(ro, rd);
    
    if (hit.distance < 100.0) {
        
        vec3 lightDir = normalize(vec3(1.0, 1.0, 1.0));
        vec3 normal = calcNormal(hit.position);
        float diff = max(dot(normal, lightDir), 0.0);
        
        
        vec3 materialColor = getMaterialColor(hit.material, hit.position);
        
        
        vec3 color = materialColor * (diff + 0.2);
        
        
        if (hit.material == MATERIAL_METAL) {
            vec3 reflectDir = reflect(rd, normal);
            HitInfo reflectHit = raymarch(hit.position + normal * 0.01, reflectDir);
            if (reflectHit.distance < 100.0) {
                vec3 reflectColor = getMaterialColor(reflectHit.material, reflectHit.position);
                color = mix(color, reflectColor, 0.8);
            }
        }
        
        FragColor = vec4(color, 1.0);
    } else {
        
        vec3 sky = mix(vec3(0.5, 0.7, 1.0), vec3(1.0), smoothstep(-0.2, 0.5, rd.y));
        FragColor = vec4(sky, 1.0);
    }
}
    ";

    // Компиляция шейдеров с проверкой ошибок
    int vertexShader = GL.CreateShader(ShaderType.VertexShader);
    GL.ShaderSource(vertexShader, vertexShaderSource);
    GL.CompileShader(vertexShader);
    
    // Проверка компиляции вершинного шейдера
    GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int vertexStatus);
    if (vertexStatus == 0)
    {
        Console.WriteLine("Vertex shader error: " + GL.GetShaderInfoLog(vertexShader));
    }

    int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
    GL.ShaderSource(fragmentShader, fragmentShaderSource);
    GL.CompileShader(fragmentShader);
    
    // Проверка компиляции фрагментного шейдера
    GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out int fragmentStatus);
    if (fragmentStatus == 0)
    {
        Console.WriteLine("Fragment shader error: " + GL.GetShaderInfoLog(fragmentShader));
    }

    // Создание шейдерной программы
    int program = GL.CreateProgram();
    GL.AttachShader(program, vertexShader);
    GL.AttachShader(program, fragmentShader);
    GL.LinkProgram(program);
    
    // Проверка линковки программы
    GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int programStatus);
    if (programStatus == 0)
    {
        Console.WriteLine("Program link error: " + GL.GetProgramInfoLog(program));
    }

    GL.DetachShader(program, vertexShader);
    GL.DetachShader(program, fragmentShader);
    GL.DeleteShader(vertexShader);
    GL.DeleteShader(fragmentShader);
    
    return program;
}
}