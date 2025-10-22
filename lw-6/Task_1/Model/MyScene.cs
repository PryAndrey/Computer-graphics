using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Drawing;


internal class MyScene
{
    public struct Object3D
    {
        public Vector3 Position;
        public Model Model;
        public float Scale;
        public float Rotation;
        public float Rotation2;

        public Object3D(Model model, Vector3 position, float scale, float rotation, float rotation2 = 0)
        {
            Model = model;
            Position = position;
            Scale = scale;
            Rotation = rotation;
            Rotation2 = rotation2;
        }
    }

    private readonly MaterialLoader _loader = new MaterialLoader();

    private readonly Model _car1;
    private readonly Model _car2;
    private readonly Model _car3;
    private readonly Model _car4;
    private readonly Model _house1;
    private readonly Model _house2;
    private readonly Model _house3;
    private readonly Model _tree1;
    private readonly Model _light1;
    
    private readonly Object3D[] _objects1;
    private readonly Object3D[] _objects2;
    private readonly Object3D[] _objects3;

    private readonly int _floorTexture, _roadTexture;
    
    public MyScene()
    {
        _car1 = LoadModel("models/police_car.3ds");
        _car2 = LoadModel("models/supercar.3ds");
        _car3 = LoadModel("models/car2.3ds");
        _car4 = LoadModel("models/tractor.3ds");

        _house1 = LoadModel("models/house1.3ds");
        _house2 = LoadModel("models/house2.3ds");
        _house3 = LoadModel("models/house3.3ds");

        _tree1 = LoadModel("models/low_poly_savana_tree.glb");
        _light1 = LoadModel("models/untitled.glb");

        _floorTexture = _loader.GetTextureId("grass.jpg");
        _roadTexture = _loader.GetTextureId("roadstrip.jpg");


        _objects1 = new Object3D[]
        {
            new(_house1, new Vector3(3f, 0.6f, -4), 1f, 0f),
            new(_house2, new Vector3(3f, 0.9f, -8), 1f, 0f),
            new(_house3, new Vector3(5.5f, 0.6f, -4), 1f, 90f),
            new(_house2, new Vector3(8f, 0.9f, -4), 1f, 90f),

            new(_house1, new Vector3(-3f, 0.6f, -4), 1f, 0f),
            new(_house1, new Vector3(-3f, 0.9f, -8), 1f, 0f),
            new(_house1, new Vector3(-5.5f, 0.6f, -4), 1f, 90f),
            new(_house3, new Vector3(-8f, 0.9f, -4), 1f, 90f),

            new(_house3, new Vector3(-3f, 0.6f, 5), 1f, 180f),
            new(_house1, new Vector3(-3f, 0.9f, 8), 1f, 0f),
            new(_house1, new Vector3(-5.5f, 0.6f, 4), 1f, 90f),
            new(_house3, new Vector3(-8f, 0.9f, 4), 1f, 90f),
        };

        _objects2 = new Object3D[]
        {
            // Деревья
            new(_tree1, new Vector3(2f, 0f, 2f), 0.15f, 0, -90f),
            new(_tree1, new Vector3(4f, 0f, 2f), 0.1f, 50, -90f),
            new(_tree1, new Vector3(6f, 0f, 2f), 0.1f, 50, -90f),
            new(_tree1, new Vector3(8f, 0f, 2f), 0.14f, 0, -90f),
            new(_tree1, new Vector3(9f, 0f, 2f), 0.1f, 70, -90f),
            new(_tree1, new Vector3(2f, 0f, 2f), 0.17f, 0, -90f),
            new(_tree1, new Vector3(2f, 0f, 4f), 0.1f, 90, -90f),
            new(_tree1, new Vector3(2f, 0f, 6f), 0.1f, 230, -90f),
            new(_tree1, new Vector3(2f, 0f, 8f), 0.1f, 0, -90f),
            new(_tree1, new Vector3(2f, 0f, 9f), 0.15f, 0, -90f),

            new(_tree1, new Vector3(-2f, 0f, 2f), 0.1f, 0, -90f),
            new(_tree1, new Vector3(-4f, 0f, 2f), 0.1f, 50, -90f),
            new(_tree1, new Vector3(-6f, 0f, 2f), 0.14f, 50, -90f),
            new(_tree1, new Vector3(-8f, 0f, 2f), 0.1f, 0, -90f),
            new(_tree1, new Vector3(-9f, 0f, 2f), 0.16f, 70, -90f),
            new(_tree1, new Vector3(-2f, 0f, 2f), 0.1f, 0, -90f),
            new(_tree1, new Vector3(-2f, 0f, 4f), 0.12f, 90, -90f),
            new(_tree1, new Vector3(-2f, 0f, 6f), 0.1f, 230, -90f),
            new(_tree1, new Vector3(-2f, 0f, 8f), 0.11f, 0, -90f),
            new(_tree1, new Vector3(-2f, 0f, 9f), 0.1f, 0, -90f),


            new(_tree1, new Vector3(2f, 0f, -2f), 0.1f, 0, -90f),
            new(_tree1, new Vector3(4f, 0f, -2f), 0.15f, 50, -90f),
            new(_tree1, new Vector3(6f, 0f, -2f), 0.1f, 50, -90f),
            new(_tree1, new Vector3(8f, 0f, -2f), 0.14f, 0, -90f),
            new(_tree1, new Vector3(9f, 0f, -2f), 0.1f, 70, -90f),
            new(_tree1, new Vector3(2f, 0f, -2f), 0.1f, 0, -90f),
            new(_tree1, new Vector3(2f, 0f, -4f), 0.16f, 90, -90f),
            new(_tree1, new Vector3(2f, 0f, -6f), 0.1f, 230, -90f),
            new(_tree1, new Vector3(2f, 0f, -8f), 0.14f, 0, -90f),
            new(_tree1, new Vector3(2f, 0f, -9f), 0.1f, 0, -90f),

            new(_tree1, new Vector3(-2f, 0f, -2f), 0.1f, 0, -90f),
            new(_tree1, new Vector3(-4f, 0f, -2f), 0.1f, 50, -90f),
            new
            (_tree1, new Vector3(-6f, 0f, -2f), 0.12f, 50, -90f
            ),
            new(_tree1, new Vector3(-8f, 0f, -2f), 0.14f, 0, -90f),
            new(_tree1, new Vector3(-9f, 0f, -2f), 0.1f, 70, -90f),
            new(_tree1, new Vector3(-2f, 0f, -2f), 0.14f, 0, -90f),
            new(_tree1, new Vector3(-2f, 0f, -4f), 0.1f, 90, -90f),
            new(_tree1, new Vector3(-2f, 0f, -6f), 0.13f, 230, -90f),
            new(_tree1, new Vector3(-2f, 0f, -8f), 0.1f, 0, -90f),
            new(_tree1, new Vector3(-2f, 0f, -9f), 0.1f, 0, -90f),

            // лес
            new(_tree1, new Vector3(3f, 0f, 2f), 0.1f, 0, -90f),
            new(_tree1, new Vector3(4f, 0f, 3f), 0.1f, 50, -90f),
            new(_tree1, new Vector3(5f, 0f, 4f), 0.15f, 50, -90f),
            new(_tree1, new Vector3(6f, 0f, 2f), 0.1f, 0, -90f),
            new(_tree1, new Vector3(7f, 0f, 4f), 0.17f, 70, -90f),
            new(_tree1, new Vector3(8f, 0f, 2f), 0.1f, 0, -90f),
            new(_tree1, new Vector3(9f, 0f, 3f), 0.12f, 90, -90f),

            new(_tree1, new Vector3(3f, 0f, 6f), 0.17f, 0, -90f),
            new(_tree1, new Vector3(4f, 0f, 5f), 0.1f, 50, -90f),
            new(_tree1, new Vector3(5f, 0f, 6f), 0.1f, 50, -90f),
            new(_tree1, new Vector3(6f, 0f, 8f), 0.13f, 0, -90f),
            new(_tree1, new Vector3(7f, 0f, 9f), 0.1f, 70, -90f),
            new(_tree1, new Vector3(8f, 0f, 2f), 0.1f, 0, -90f),
            new(_tree1, new Vector3(9f, 0f, 8f), 0.11f, 90, -90f),

            new(_tree1, new Vector3(3f, 0f, 8f), 0.1f, 0, -90f),
            new(_tree1, new Vector3(4f, 0f, 5f), 0.1f, 50, -90f),
            new(_tree1, new Vector3(5f, 0f, 4f), 0.1f, 50, -90f),
            new(_tree1, new Vector3(6f, 0f, 8f), 0.18f, 0, -90f),
            new(_tree1, new Vector3(7f, 0f, 4f), 0.1f, 70, -90f),
            new(_tree1, new Vector3(8f, 0f, 8f), 0.1f, 0, -90f),
            new(_tree1, new Vector3(9f, 0f, 6f), 0.13f, 90, -90f),

            new(_tree1, new Vector3(3f, 0f, 4f), 0.1f, 0, -90f),
            new(_tree1, new Vector3(4f, 0f, 8f), 0.14f, 50, -90f),
            new(_tree1, new Vector3(5f, 0f, 3f), 0.1f, 70, -90f),
            new(_tree1, new Vector3(6f, 0f, 7f), 0.18f, 30, -90f),
            new(_tree1, new Vector3(7f, 0f, 2f), 0.1f, 40, -90f),
            new(_tree1, new Vector3(8f, 0f, 4f), 0.19f, 0, -90f),
            new(_tree1, new Vector3(9f, 0f, 7f), 0.13f, 40, -90f),

            // Светофоры
            new(_light1, new Vector3(1f, 1.2f, 1), 0.08f, 0, -90f),
            new(_light1, new Vector3(1f, 1.2f, 1), 0.08f, 90, -90f),
            new(_light1, new Vector3(1f, 1.2f, 1), 0.08f, 180, -90f),
            new(_light1, new Vector3(1f, 1.2f, 1), 0.08f, 270, -90f),

            new(_light1, new Vector3(-1f, 1.2f, 1), 0.08f, 0, -90f),
            new(_light1, new Vector3(-1f, 1.2f, 1), 0.08f, 90, -90f),
            new(_light1, new Vector3(-1f, 1.2f, 1), 0.08f, 180, -90f),
            new(_light1, new Vector3(-1f, 1.2f, 1), 0.08f, 270, -90f),

            new(_light1, new Vector3(1f, 1.2f, -1), 0.08f, 0, -90f),
            new(_light1, new Vector3(1f, 1.2f, -1), 0.08f, 90, -90f),
            new(_light1, new Vector3(1f, 1.2f, -1), 0.08f, 180, -90f),
            new(_light1, new Vector3(1f, 1.2f, -1), 0.08f, 270, -90f),
            new(_light1, new Vector3(-1f, 1.2f, -1), 0.08f, 0, -90f),
            new(_light1, new Vector3(-1f, 1.2f, -1), 0.08f, 90, -90f),
            new(_light1, new Vector3(-1f, 1.2f, -1), 0.08f, 180, -90f),
            new(_light1, new Vector3(-1f, 1.2f, -1), 0.08f, 270, -90f)
        };

        _objects3 = new Object3D[]
        {
            new(_car1, new Vector3(-0.3f, 0.2f, -2), 0.5f, 0f),
            new(_car2, new Vector3(6f, 0.2f, 0.3f), 0.5f, 0f),
            new(_car3, new Vector3(0.3f, 0.25f, 9), 0.5f, 0f),
            new(_car4, new Vector3(5f, 0.3f, -0.3f), 0.4f, -90f),
        };
    }

    private Model LoadModel(string path)
    {
        var m = new Model();
        m.LoadModel(path);
        return m;
    }

    public void Update(float delta)
    {
        Vector3[] directions = new[]
        {
            Vector3.UnitZ,
            Vector3.UnitX,
            -Vector3.UnitZ,
            -Vector3.UnitX,
        };

        const float limit = 10f;
        float speed = 2f;

        for (int i = 0; i < _objects3.Length; i++)
        {
            var obj = _objects3[i];
            Vector3 direction = directions[i];

            if (i == 3 && obj.Position.X < 0 && obj.Rotation < 0)
            {
                obj.Rotation += speed;
            }

            if (i == 3 && obj.Position.X < -0.3f)
            {
                direction = Vector3.UnitZ;
            }

            obj.Position += direction * speed * delta;

            if (direction == Vector3.UnitX && obj.Position.X > limit)
            {
                obj.Position = new Vector3(-limit, obj.Position.Y, obj.Position.Z);
            }
            else if (direction == -Vector3.UnitX && obj.Position.X < -limit)
            {
                obj.Position = new Vector3(limit, obj.Position.Y, obj.Position.Z);
            }
            else if (direction == Vector3.UnitZ && obj.Position.Z > limit)
            {
                if (i == 3)
                {
                    obj.Position = new Vector3(limit, obj.Position.Y, -0.3f);
                    obj.Rotation = -90;
                }
                else
                    obj.Position = new Vector3(obj.Position.X, obj.Position.Y, -limit);
            }
            else if (direction == -Vector3.UnitZ && obj.Position.Z < -limit)
            {
                obj.Position = new Vector3(obj.Position.X, obj.Position.Y, limit);
            }

            _objects3[i] = obj;
        }
    }

    public void Draw()
    {
        GL.PushMatrix();
        DrawField();

        foreach (var object3D in _objects1)
        {
            DrawObject(object3D.Model, object3D.Position, object3D.Scale, object3D.Rotation, Vector3.UnitY);
        }

        foreach (var object3D in _objects2)
        {
            DrawObjectRotate(object3D.Model, object3D.Position, object3D.Scale, object3D.Rotation, object3D.Rotation2,
                Vector3.UnitZ);
        }

        foreach (var object3D in _objects3)
        {
            DrawObject(object3D.Model, object3D.Position, object3D.Scale, object3D.Rotation, Vector3.UnitY);
        }

        GL.PopMatrix();
    }

    private void DrawField()
    {
        GL.Enable(EnableCap.Texture2D);
        GL.BindTexture(TextureTarget.Texture2D, _floorTexture);

        GL.Begin(PrimitiveType.Quads);
        GL.Color3(Color.White);

        float size = 10f;
        float repeat = 10f;

        GL.TexCoord2(0, 0);
        GL.Vertex3(-size, 0, -size);
        GL.TexCoord2(0, repeat);
        GL.Vertex3(-size, 0, size);
        GL.TexCoord2(repeat, repeat);
        GL.Vertex3(size, 0, size);
        GL.TexCoord2(repeat, 0);
        GL.Vertex3(size, 0, -size);

        GL.End();
        GL.Disable(EnableCap.Texture2D);

        DrawRoad(0f, 0.01f, 0f, 1.5f, 20f);
        DrawRoad(0f, 0.02f, 0f, 1.5f, 20f, 90);
    }

    private void DrawRoad(float centerX, float centerY, float centerZ,
        float width, float length, float rotation = 0f)
    {
        GL.Enable(EnableCap.Texture2D);
        GL.BindTexture(TextureTarget.Texture2D, _roadTexture);

        GL.PushMatrix();
        GL.Translate(centerX, centerY, centerZ);
        if (rotation != 0f) GL.Rotate(rotation, Vector3.UnitY);

        GL.Begin(PrimitiveType.Quads);

        float halfWidth = width / 2;
        float halfLength = length / 2;
        float texRepeat = length / width;

        GL.TexCoord2(0, 0);
        GL.Vertex3(-halfWidth, 0, -halfLength);

        GL.TexCoord2(0, texRepeat);
        GL.Vertex3(-halfWidth, 0, halfLength);

        GL.TexCoord2(1, texRepeat);
        GL.Vertex3(halfWidth, 0, halfLength);

        GL.TexCoord2(1, 0);
        GL.Vertex3(halfWidth, 0, -halfLength);

        GL.End();
        GL.PopMatrix();

        GL.Disable(EnableCap.Texture2D);
    }

    private void DrawObject(Model model, Vector3 position, float scale, float rotationDegrees, Vector3 rotationAxis)
    {
        GL.PushMatrix();
        GL.Translate(position);
        GL.Rotate(rotationDegrees, rotationAxis);
        GL.Scale(scale, scale, scale);
        model.RenderModel();
        GL.PopMatrix();
    }

    private void DrawObjectRotate(Model model, Vector3 position, float scale, float rotationDegrees,
        float rotationDegrees2, Vector3 rotationAxis)
    {
        GL.PushMatrix();
        GL.Translate(position);
        GL.Rotate(rotationDegrees2, Vector3.UnitX);
        GL.Rotate(rotationDegrees, Vector3.UnitZ);
        GL.Scale(scale, scale, scale);
        model.RenderModel();
        GL.PopMatrix();
    }
}