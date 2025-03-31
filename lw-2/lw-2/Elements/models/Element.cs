using Elements.models;

public class Element
{
    public ElementType Type { get; private set; }

    public int Id { get; private set; }
    public int X { get; set; }
    public int Y { get; set; }

    public Element(ElementType type, int x = 0, int y = 0)
    {
        Id = ElementIdGenerator.GetNextId();
        Type = type;
        X = x;
        Y = y;
    }
}