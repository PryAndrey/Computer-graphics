using Elements.models;

public enum ElementChangeType
{
    Add,
    Remove
}

public class ElementChangeEventArgs
{
    public int ChangedElementId { get; }

    public ElementChangeType ChangeType { get; }

    public ElementChangeEventArgs(int changedElementId, ElementChangeType type)
    {
        ChangedElementId = changedElementId;
        ChangeType = type;
    }
}

public enum GameState
{
    Playing,
    End
}

public class GameModel
{
    private const int MaxElements = 20;
    private static readonly int ElementsToWin = Enum.GetValues(typeof(ElementType)).Length;
    public GameState State { get; private set; }
    public List<ElementType> OpenElements { get; private set; }
    public List<Element> CurrentElements { get; private set; }

    public event EventHandler? OpenElementsChanged;
    public event EventHandler<ElementChangeEventArgs>? CurrentElementsChanged;
    public event EventHandler StateChanged;

    public GameModel()
    {
        OpenElements = new List<ElementType>();
        CurrentElements = new List<Element>();
    }

    public void Start()
    {
        State = GameState.Playing;
        InitializeElements();
    }

    public void AddElement(ElementType type, int x, int y)
    {
        if (State == GameState.End
            || CurrentElements.Count >= MaxElements
            || !OpenElements.Contains(type))
            return;

        var newElement = new Element(type, x, y);

        CurrentElements.Add(newElement);

        CurrentElementsChanged?.Invoke(this, new ElementChangeEventArgs(newElement.Id, ElementChangeType.Add));
    }

    public void RemoveElement(int id)
    {
        var element = CurrentElements.FirstOrDefault(e => e.Id == id);

        if (!CurrentElements.Contains(element))
            return;

        CurrentElements.Remove(element);

        CurrentElementsChanged?.Invoke(this, new ElementChangeEventArgs(element.Id, ElementChangeType.Remove));
    }
    
    public void MoveElement(int id, int x, int y)
    {
        var element = CurrentElements.FirstOrDefault(e => e.Id == id);

        if (!CurrentElements.Contains(element))
            return;
        element.X = x;
        element.Y = y;

        CurrentElementsChanged?.Invoke(this, new ElementChangeEventArgs(element.Id, ElementChangeType.Remove));
    }

    public bool TryCombineElements(int id1, int id2)
    {
        var element1 = CurrentElements.FirstOrDefault(e => e.Id == id1);
        var element2 = CurrentElements.FirstOrDefault(e => e.Id == id2);

        if (!CurrentElements.Contains(element1) && !CurrentElements.Contains(element2))
            return false;

        ElementType? result = ElementCombinations.GetCombinationResult(element1.Type, element2.Type);

        if (result is null)
            return false;

        if (!OpenElements.Contains(result.Value))
        {
            OpenElements.Add(result.Value);
            OpenElementsChanged?.Invoke(this, EventArgs.Empty);
        }

        RemoveElement(id1);
        RemoveElement(id2);

        AddElement(result.Value, element2.X, element2.Y);

        UpdateState();

        return true;
    }

    public void SortOpenElements()
    {
        OpenElements.Sort();
        OpenElementsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void UpdateState()
    {
        if (State == GameState.Playing && OpenElements.Count >= 6)
        {
            State = GameState.End;

            StateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void InitializeElements()
    {
        OpenElements.Clear();
        CurrentElements.Clear();

        var initialElements = new[]
        {
            (ElementType: ElementType.Air, X: 100, Y: 100),
            (ElementType: ElementType.Earth, X: 150, Y: 100),
            (ElementType: ElementType.Fire, X: 200, Y: 100),
            (ElementType: ElementType.Water, X: 250, Y: 100)
        };
        foreach (var elementInfo in initialElements)
        {
            var element = new Element(elementInfo.ElementType, elementInfo.X, elementInfo.Y);
            OpenElements.Add(elementInfo.ElementType);
            CurrentElements.Add(element);
            CurrentElementsChanged?.Invoke(this, new ElementChangeEventArgs(element.Id, ElementChangeType.Add));
        }

        OpenElementsChanged?.Invoke(this, EventArgs.Empty);
    }
}