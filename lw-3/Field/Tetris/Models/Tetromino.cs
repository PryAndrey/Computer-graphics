using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;

public enum TetrominoType
{
    Stick,
    ReverseL,
    L,
    Square,
    S,
    ReverseS,
    Piramid
}

public enum Direction
{
    Left,
    Right,
    Down
}

public class Tetromino
{
    public TetrominoType Type { get; }
    public Color?[,] Blocks { get; private set; }
    public Color Color { get; }
    public Point Position { get; set; }

    public Tetromino(TetrominoType type, Point position)
    {
        Type = type;
        Position = position;
        Blocks = GetBlocks(type);
        Color = TetrominoColors[type];
    }

    public Tetromino(Tetromino other)
    {
        Type = other.Type;
        Position = other.Position;
        Blocks = other.Blocks;
        Color = other.Color;
    }

    public void Rotate()
    {
        int rows = Blocks.GetLength(0);
        int cols = Blocks.GetLength(1);
        Color?[,] rotatedBlocks = new Color?[cols, rows];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                rotatedBlocks[j, rows - 1 - i] = Blocks[i, j];
            }
        }

        Blocks = rotatedBlocks;
    }

    public void Move(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                Position = new Point(Position.X - 1, Position.Y);
                break;
            case Direction.Right:
                Position = new Point(Position.X + 1, Position.Y);
                break;
            case Direction.Down:
                Position = new Point(Position.X, Position.Y + 1);
                break;
            default:
                throw new ArgumentOutOfRangeException("Invalid direction");
        }
    }

    public int GetWidth()
    {
        return Blocks.GetLength(1);
    }

    public int GetHeight()
    {
        return Blocks.GetLength(0);
    }

    private static Color?[,] GetBlocks(TetrominoType type)
    {
        var c = TetrominoColors[type];
        var form = TetrominoForms[type];

        int rows = form.GetLength(0);
        int cols = form.GetLength(1);

        var blocks = new Color?[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                blocks[i, j] = form[i, j] == 1 ? c : null;
            }
        }

        return blocks;
    }

    private static readonly Dictionary<TetrominoType, int[,]> TetrominoForms = new()
    {
        {
            TetrominoType.L, new[,]
            {
                { 1, 0 },
                { 1, 0 },
                { 1, 1 }
            }
        },
        {
            TetrominoType.ReverseL, new[,]
            {
                { 0, 1 },
                { 0, 1 },
                { 1, 1 }
            }
        },
        {
            TetrominoType.ReverseS, new[,]
            {
                { 1, 1, 0 },
                { 0, 1, 1 }
            }
        },
        {
            TetrominoType.S, new[,]
            {
                { 0, 1, 1 },
                { 1, 1, 0 }
            }
        },
        {
            TetrominoType.Stick, new[,]
            {
                { 1 }, { 1 }, { 1 }, { 1 }
            }
        },
        {
            TetrominoType.Square, new[,]
            {
                { 1, 1 },
                { 1, 1 }
            }
        },
        {
            TetrominoType.Piramid, new[,]
            {
                { 0, 1, 0 },
                { 1, 1, 1 }
            }
        }
    };

    private static readonly Dictionary<TetrominoType, Color> TetrominoColors = new()
    {
        { TetrominoType.L, Color.GreenYellow },
        { TetrominoType.ReverseL, Color.Green },
        { TetrominoType.ReverseS, Color.Chocolate },
        { TetrominoType.S, Color.Purple },
        { TetrominoType.Stick, Color.Red },
        { TetrominoType.Square, Color.LightGreen },
        { TetrominoType.Piramid, Color.LightBlue }
    };
}