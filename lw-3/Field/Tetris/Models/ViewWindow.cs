using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Drawing;
using Tetris;
using Tetris.Models;
using Tetris.Shaders;

public class ViewWindow : GameWindow
{
    private Shader _primitiveShader;
    private Shader _textureShader;

    private Matrix4 _projection;

    private GameModel _gameModel;
    private TextRenderer _textRenderer;

    private int _vertexBufferObject;
    private int _elementBufferObject;

    private static readonly float CellSize = 0.09f;
    private static readonly float BlockPadding = 0.01f;
    private static readonly float BoardStartX = -1.0f;
    private static readonly float BoardStartY = -1.0f;

    private readonly float[] _textureVertices =
    {
        0.1f, 0.7f, 0f, 0.0f, 0.0f,
        1.0f, 0.7f, 0f, 1.0f, 0.0f,
        1.0f, -1.0f, 0f, 1.0f, 1.0f,
        0.1f, -1.0f, 0f, 0.0f, 1.0f
    };

    private readonly uint[] _textureIndices = { 0, 1, 3, 1, 2, 3 };

    public ViewWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(Color.White);

        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

        _elementBufferObject = GL.GenBuffer();

        int vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(vertexArrayObject);

        _primitiveShader = new Shader("../../../Shaders/prim.vert", "../../../Shaders/prim.frag");
        _textureShader = new Shader("../../../Shaders/tex.vert", "../../../Shaders/tex.frag");

        InitializeTextRenderer();

        UpdateOrthographicMatrix();

        _gameModel = new GameModel();
        _gameModel.Start();
    }

    private void InitializeTextRenderer()
    {
        GL.Enable(EnableCap.Texture2D);
        _textRenderer = new TextRenderer(700, 1300);
        _textRenderer.Clear(Color.White);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);

        _primitiveShader.Use();

        DrawGameBoard();
        DrawInformation();

        SwapBuffers();

        base.OnRenderFrame(args);
    }

    private void DrawNextTetromino()
    {
        float startX = 0.3f;
        float startY = 0.4f;

        for (int i = 0; i < _gameModel.Board.NextTetromino.GetWidth(); i++)
        {
            for (int j = 0; j < _gameModel.Board.NextTetromino.GetHeight(); j++)
            {
                if (_gameModel.Board.NextTetromino.Blocks[j, i] == null)
                    continue;

                float x1 = startX + i * (CellSize + BlockPadding);
                float y1 = startY + (CellSize - j - 1) * (CellSize + BlockPadding);
                float x2 = x1 + CellSize;
                float y2 = y1 + CellSize;

                var color = ToColor4Converter.Convert(_gameModel.Board.NextTetromino.Blocks[j, i]);

                List<RGBVertex> vertices =
                [
                    new(x1, y1, color),
                    new(x1, y2, color),
                    new(x2, y2, color),
                    new(x2, y1, color)
                ];

                DrawPrimitiveVertices(vertices, PrimitiveType.Quads);
            }
        }
    }

    private void DrawGameBoard()
    {
        for (int x = 0; x < GameBoard.Width; x++)
        {
            for (int y = 0; y < GameBoard.Height; y++)
            {
                float x1 = BoardStartX + x * (CellSize + BlockPadding);
                float y1 = BoardStartY + (GameBoard.Height - y - 1) * (CellSize + BlockPadding);
                float x2 = x1 + CellSize;
                float y2 = y1 + CellSize;

                var color = ToColor4Converter.Convert(
                    _gameModel.Board.Blocks[y, x] != null
                        ? _gameModel.Board.Blocks[y, x]
                        : GameBoard.BoardColor);

                List<RGBVertex> vertices =
                [
                    new(x1, y1, color),
                    new(x1, y2, color),
                    new(x2, y2, color),
                    new(x2, y1, color),
                    new(x1, y1, color)
                ];

                DrawPrimitiveVertices(vertices, PrimitiveType.Polygon);
            }
        }
    }

    private void DrawPrimitiveVertices(List<RGBVertex> list, PrimitiveType mode)
    {
        var array = list.SelectMany(v => RGBVertex.ToFloatArray(v)).ToArray();

        _primitiveShader.Use();

        // Загружаем данные в VBO
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, array.Length * sizeof(float), array, BufferUsageHint.DynamicDraw);

        // Настройка атрибутов вершин
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, RGBVertex.VertexSize * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, RGBVertex.VertexSize * sizeof(float), RGBVertex.ColorIndex * sizeof(float));
        GL.EnableVertexAttribArray(1);

        GL.DrawArrays(mode, 0, array.Length / 6);
    }

    private void DrawInformation()
    {
        List<string> lines = _gameModel.State == GameState.GameOver
            ?
            [
                "Game over!",
                $"Score: {_gameModel.Score}"
            ]
            : _gameModel.State == GameState.Paused
                ?
                [
                    "'P' to resume."
                ]
                :
                [
                    $"Level: {_gameModel.Level}",
                    $"Score: {_gameModel.Score}",
                    $"Lines left: {_gameModel.LinesToNextLevel}",
                    "Next: "
                ];

        RenderText(lines);

        if (_gameModel.State == GameState.Running)
        {
            DrawNextTetromino();
        }
    }

    private void RenderText(List<string> lines)
    {
        _textRenderer.Clear(Color.White);

        foreach (var line in lines)
        {
            _textRenderer.DrawNewString(line, Brushes.Black);
        }

        DrawText();
    }

    private void DrawText()
    {
        _textureShader.Use();
        _textRenderer.Use();

        // Загружаем данные в VBO и EBO
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _textureVertices.Length * sizeof(float), _textureVertices, BufferUsageHint.DynamicDraw);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _textureIndices.Length * sizeof(uint), _textureIndices, BufferUsageHint.DynamicDraw);

        // Настройка атрибутов вершин
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        GL.DrawElements(PrimitiveType.Triangles, _textureIndices.Length, DrawElementsType.UnsignedInt, 0);
    }

    private void UpdateOrthographicMatrix()
    {
        float aspectRatio = (float)Size.X / Size.Y;

        _projection = aspectRatio > 1
            ? Matrix4.CreateOrthographic(aspectRatio * 2.0f, 2.0f, -1.0f, 1.0f)
            : Matrix4.CreateOrthographic(2.0f, 2.0f / aspectRatio, -1.0f, 1.0f);

        _primitiveShader.SetMatrix4("projection", _projection);
    }

    protected override void OnKeyDown(KeyboardKeyEventArgs e)
    {
        base.OnKeyDown(e);

        switch (e.Key)
        {
            case OpenTK.Windowing.GraphicsLibraryFramework.Keys.Left:
                _gameModel.Move(Direction.Left);
                break;
            case OpenTK.Windowing.GraphicsLibraryFramework.Keys.Right:
                _gameModel.Move(Direction.Right);
                break;
            case OpenTK.Windowing.GraphicsLibraryFramework.Keys.Down:
                _gameModel.Move(Direction.Down);
                break;
            case OpenTK.Windowing.GraphicsLibraryFramework.Keys.Up:
                _gameModel.Rotate();
                break;
            case OpenTK.Windowing.GraphicsLibraryFramework.Keys.P:
                _gameModel.Pause();
                break;
        }
    }

    protected override void OnUnload()
    {
        base.OnUnload();

        _primitiveShader.Dispose();
        _textRenderer.Dispose();
        _gameModel.Dispose();
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);

        GL.Viewport(0, 0, e.Width, e.Height);
        UpdateOrthographicMatrix();
    }
}