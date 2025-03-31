using System.ComponentModel;

public class Presenter
{
    private readonly IView _view;
    private readonly Model _model;

    public Presenter(IView view, Model model)
    {
        _view = view;
        _model = model;

        _view.SetPresenter(this);
        _model.PropertyChanged += Model_PropertyChanged;

        StartNewGame();
    }

    public bool OnLetterClicked(char letter)
    {
        bool correctGuess = _model.GuessLetter(letter);

        if (_model.IsGameWon())
        {
            _view.ShowGameOverMessage(true);
            StartNewGame();
        }
        else if (_model.IsGameOver())
        {
            _view.ShowGameOverMessage(false);
            StartNewGame();
        }

        return correctGuess;
    }

    private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Model.CurrentDisplayWord) ||
            e.PropertyName == nameof(Model.RemainingAttempts))
        {
            _view.UpdateView(_model.CurrentDisplayWord, _model.Hint, _model.RemainingAttempts);
        }
    }

    public void StartNewGame()
    {
        _model.LoadNewWord("../../../words.txt");
        _view.RegenerateView(_model.CurrentDisplayWord, _model.Hint, _model.RemainingAttempts);
    }
}