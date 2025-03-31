using System;
using System.Drawing;
using System.Windows.Forms;

public interface IView
{
    void SetPresenter(Presenter presenter);
    void RegenerateView(string displayWord, string hint, int remainingAttempts);
    void UpdateView(string displayWord, string hint, int remainingAttempts);
    void ShowGameOverMessage(bool won);
}

public class View : Form, IView
{
    private Presenter _presenter;
    private Label _wordLabel;
    private Label _hintLabel;
    private Label _attemptsLabel;
    private FlowLayoutPanel _lettersPanel;
    private Button _switchViewButton;
    private bool _isAlternativeView;
    private int _remainingAttempts;

    public View()
    {
        this.ClientSize = new Size(600, 400);

        _wordLabel = new Label { Location = new Point(10, 10), AutoSize = true };
        _hintLabel = new Label { Location = new Point(10, 40), AutoSize = true };
        _attemptsLabel = new Label { Location = new Point(10, 70), AutoSize = true };
        _lettersPanel = new FlowLayoutPanel
        {
            Location = new Point(10, 100),
            AutoSize = true,
            WrapContents = true,
            FlowDirection = FlowDirection.LeftToRight
        };
        _switchViewButton = new Button { Text = "Switch View", Location = new Point(10, 300) };

        _switchViewButton.Click += (sender, args) => SwitchView();

        Controls.Add(_wordLabel);
        Controls.Add(_hintLabel);
        Controls.Add(_attemptsLabel);
        Controls.Add(_lettersPanel);
        Controls.Add(_switchViewButton);

        GenerateLetterButtons();
        this.Paint += View_Paint;
    }

    private void GenerateLetterButtons()
    {
        _lettersPanel.Controls.Clear();
        int lettersPerRow = 13;

        for (char c = 'A'; c <= 'Z'; c++)
        {
            var button = new Button { Text = c.ToString(), Width = 30, Height = 30 };
            button.Click += (sender, args) =>
            {
                bool correctGuess = _presenter.OnLetterClicked(button.Text[0]);
                button.Enabled = false;
                button.BackColor = correctGuess ? Color.Green : Color.Red;
            };
            button.BackColor = Color.LightGray;
            _lettersPanel.Controls.Add(button);

            if ((c - 'A' + 1) % lettersPerRow == 0)
            {
                _lettersPanel.SetFlowBreak(button, true);
            }
        }

        for (char c = 'А'; c <= 'Я'; c++)
        {
            var button = new Button { Text = c.ToString(), Width = 30, Height = 30 };
            button.Click += (sender, args) =>
            {
                bool correctGuess = _presenter.OnLetterClicked(button.Text[0]);
                button.Enabled = false;
                button.BackColor = correctGuess ? Color.Green : Color.Red;
            };
            button.BackColor = Color.SeaShell;
            _lettersPanel.Controls.Add(button);

            if ((c - 'А' + 1) % lettersPerRow == 0)
            {
                _lettersPanel.SetFlowBreak(button, true);
            }
        }
    }

    public void SetPresenter(Presenter presenter)
    {
        _presenter = presenter;
    }

    public void RegenerateView(string displayWord, string hint, int remainingAttempts)
    {
        UpdateView(displayWord, hint, remainingAttempts);
        GenerateLetterButtons();
    }

    public void UpdateView(string displayWord, string hint, int remainingAttempts)
    {
        _wordLabel.Text = $"Слово: {displayWord}";
        _hintLabel.Text = $"Подсказка: {hint}";
        _attemptsLabel.Text = $"Попыток осталось: {remainingAttempts}";
        _remainingAttempts = remainingAttempts;
        this.Invalidate();
    }

    public void ShowGameOverMessage(bool won)
    {
        var message = won ? "Вы выиграли!" : "Вы проиграли!";
        var result = MessageBox.Show(message + "\nХотите запустить новую игру?", "Конец игры", MessageBoxButtons.YesNo);

        if (result == DialogResult.Yes)
        {
            _presenter.StartNewGame();
        }
        else
        {
            Application.Exit();
        }
    }

    private void RelocationView()
    {
        this.ClientSize = new Size(_isAlternativeView ? 700 : 600, 400);

        _wordLabel.Location = new Point(_isAlternativeView ? 210 : 10, 10);
        _hintLabel.Location = new Point(_isAlternativeView ? 210 : 10, 40);
        _attemptsLabel.Location = new Point(_isAlternativeView ? 210 : 10, 70);
        _lettersPanel.Location = new Point(_isAlternativeView ? 210 : 10, 100);
        _switchViewButton.Location = new Point(10, 300);
    }

    private void SwitchView()
    {
        _isAlternativeView = !_isAlternativeView;
        RelocationView();
        this.Invalidate();
    }

    private void View_Paint(object sender, PaintEventArgs e)
    {
        DrawHangman(e.Graphics, _remainingAttempts);
    }

    private void DrawHangman(Graphics g, int remainingAttempts)
    {
        int totalParts = 7;
        int partsToDraw = totalParts - remainingAttempts;
        if (!_isAlternativeView)
        {
            g.Flush();
            return;
        }

        g.DrawLine(new Pen(Color.IndianRed, 5), 15, 20, 15, 180);
        g.DrawLine(new Pen(Color.IndianRed, 5), 15, 20, 50, 20);
        Pen thickPen = new Pen(Color.Black, 3);

        if (partsToDraw >= 1)
            g.DrawLine(thickPen, 50, 20, 50, 40);

        if (partsToDraw >= 2)
        {
            SolidBrush brush = new SolidBrush(Color.Black);
            g.FillEllipse(brush, 35, 40, 30, 30);
        }

        if (partsToDraw >= 3)
        {
            g.DrawLine(thickPen, 50, 70, 50, 120);
        }

        if (partsToDraw >= 4)
        {
            g.DrawLine(thickPen, 50, 80, 30, 100);
        }

        if (partsToDraw >= 5)
        {
            g.DrawLine(thickPen, 50, 80, 70, 100);
        }

        if (partsToDraw >= 6)
        {
            g.DrawLine(thickPen, 50, 120, 30, 150);
        }

        if (partsToDraw >= 7)
        {
            g.DrawLine(thickPen, 50, 120, 70, 150);
        }
    }
}