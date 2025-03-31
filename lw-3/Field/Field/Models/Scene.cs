using System.Globalization;
using MeadowScene.Models;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Field.Models;

public class Scene
{
    private readonly List<ISceneObject> _objects;
    private readonly List<Vector2> _flowerPositions;
    private readonly Vector2i _originalSize;
    private readonly ICanvas _canvas = new Canvas();

    public Scene(int width, int height)
    {
        _objects = new List<ISceneObject>
        {
            new Sky(new Vector2i(width, height))
        };
        _flowerPositions = new List<Vector2>();
        ReadScene("..\\..\\..\\Assets\\Scene.txt");
        _originalSize = new Vector2i(width, height);
    }

    public void ReadScene(string filePath)
    {
        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        ParseScene(line);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{ex.Message}");
                    }
                }
            }

            foreach (var obj in _objects)
            {
                if (obj is Butterfly butterfly)
                {
                    butterfly.SetFlowers(ShuffleArray(_flowerPositions));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
        }
    }

    public void ParseScene(string line)
    {
        var parts = line.Trim().Split(' ');
        if (parts.Length < 3) throw new ArgumentException("Не хватает параметров");

        string objectType = parts[0];
        if (!float.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float x) ||
            !float.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out float y))
        {
            throw new ArgumentException("Неверные параметры");
        }

        Vector2 position = new Vector2(x, y);

        switch (objectType)
        {
            case "Flower":
            {
                if (parts.Length < 4)
                    break;
                Color4 color;
                if (!TryParseColor(parts[3], out color))
                    break;

                _objects.Add(CreateColoredObject(objectType, position, color));
                _flowerPositions.Add(position);
                break;
            }
            case "Butterfly":
            case "Grass":
            {
                if (parts.Length < 4)
                    break;
                Color4 color;
                if (!TryParseColor(parts[3], out color))
                    break;

                _objects.Add(CreateColoredObject(objectType, position, color));
                break;
            }
            case "Cloud":
            case "Sun":
            case "Moon":
                _objects.Add(CreateObject(objectType, position));
                break;
            default:
                throw new ArgumentException($"Неизвестный тип объекта: {objectType}");
        }
    }

    private static bool TryParseColor(string colorName, out Color4 color)
    {
        switch (colorName.ToLower())
        {
            case "red":
                color = Color4.Red;
                return true;
            case "yellow":
                color = Color4.Yellow;
                return true;
            case "blue":
                color = Color4.Blue;
                return true;
            case "pink":
                color = Color4.Pink;
                return true;
            case "purple":
                color = Color4.Purple;
                return true;
            case "orange":
                color = Color4.Orange;
                return true;
            case "green":
                color = Color4.Green;
                return true;
            case "lime":
                color = Color4.GreenYellow;
                return true;
            default:
                color = default;
                return false;
        }
    }

    private static ISceneObject CreateColoredObject(string type, Vector2 position, Color4 color)
    {
        return type switch
        {
            "Butterfly" => new Butterfly(position, color),
            "Flower" => new Flower(position, color),
            "Grass" => new Grass(position, color),
            _ => throw new ArgumentException("Неподдерживаемый тип объекта с цветом")
        };
    }

    private static ISceneObject CreateObject(string type, Vector2 position)
    {
        return type switch
        {
            "Cloud" => new Cloud(position),
            "Sun" => new Sun(position,700),
            "Moon" => new Moon(position, 700),
            _ => throw new ArgumentException("Неподдерживаемый тип объекта без цвета")
        };
    }


    public void Resize(int width, int height)
    {
        foreach (var obj in _objects)
        {
            obj.Resize(_originalSize.X, _originalSize.Y, width, height);
        }

        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadIdentity();
        GL.Ortho(0, width, 0, height, -1, 1);
        GL.MatrixMode(MatrixMode.Modelview);
    }

    public void Update(float deltaTime)
    {
        foreach (var obj in _objects)
        {
            obj.Update(deltaTime);
        }
    }

    public static List<T> ShuffleArray<T>(List<T> array)
    {
        List<T> temp = new List<T>(array);
        Random random = new Random();

        for (int i = temp.Count - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1); // Случайный индекс от 0 до i
            // Меняем местами элементы
            (temp[i], temp[j]) = (temp[j], temp[i]);
        }

        return temp;
    }

    public void Render()
    {
        foreach (var obj in _objects)
        {
            obj.Render(_canvas);
        }
    }
}