using Elements;
using Elements.models;

public class AlchemyPresenter
{
    private readonly IAlchemyView view;
    private readonly GameModel model;

    public AlchemyPresenter(IAlchemyView view, GameModel model)
    {
        this.view = view;
        this.model = model;

        this.view.OnElementsCombined += HandleElementsCombined;
        this.view.OnElementsCombinedLast += HandleElementsCombinedLast;
        this.view.OnSortRequested += HandleSortRequested;
        this.view.OnElementAdded += HandleElementAdded;
        this.view.OnElementRemoved += HandleElementRemoved;
        this.view.OnElementMove += HandleElementMove;

        this.model.OpenElementsChanged += (s, e) => view.UpdateDiscoveredElements(model.OpenElements);
        this.model.CurrentElementsChanged += (s, e) => view.UpdateCurrentElements(model.CurrentElements);
        this.model.StateChanged += HandleStateChanged;

        model.Start();
    }

    private void HandleElementsCombined(int id1, int id2)
    {
        var elem1 = model.CurrentElements.Find(e => e.Id == id1);
        var elem2 = model.CurrentElements.Find(e => e.Id == id2);

        if (elem1 != null && elem2 != null)
        {
            if (model.TryCombineElements(elem1.Id, elem2.Id))
            {
                var elem3 = model.CurrentElements.Last();
                view.ShowMessage(
                    $"Создан новый элемент: {ElementCombinations.ToString(elem1.Type)} + {ElementCombinations.ToString(elem2.Type)} = {ElementCombinations.ToString(elem3.Type)}");
            }
            else
            {
                view.ShowMessage("Комбинация не удалась.");
            }
        }
    }

    private void HandleElementsCombinedLast(int id1)
    {
        var elem1 = model.CurrentElements.Find(e => e.Id == id1);
        var elem2 = model.CurrentElements.Last();

        if (elem1 != null && elem2 != null && elem1.Id != elem2.Id)
        {
            if (model.TryCombineElements(elem1.Id, elem2.Id))
            {
                var elem3 = model.CurrentElements.Last();
                view.ShowMessage(
                    $"Создан новый элемент: {ElementCombinations.ToString(elem1.Type)} + {ElementCombinations.ToString(elem2.Type)} = {ElementCombinations.ToString(elem3.Type)}");
            }
            else
            {
                view.ShowMessage("Комбинация не удалась.");
            }
        }
    }

    private void HandleSortRequested()
    {
        model.SortOpenElements();
    }

    private void HandleElementAdded(ElementType type, int x, int y)
    {
        model.AddElement(type, x, y);
    }

    private void HandleElementRemoved(int id)
    {
        model.RemoveElement(id);
    }

    private void HandleElementMove(int id, int x, int y)
    {
        model.MoveElement(id, x, y);
    }

    private void HandleStateChanged(object sender, EventArgs e)
    {
        if (model.State == GameState.End)
        {
            view.DisplayEndGameMessage();
        }
    }
}