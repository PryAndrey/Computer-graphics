using WordsGame;

namespace WordsGame;

static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        var model = new Model();
        var viewForm = new View();
        var presenter = new Presenter(viewForm, model);

        Application.Run(viewForm);
    }
}