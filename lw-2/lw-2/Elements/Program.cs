namespace Elements;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        GameModel model = new GameModel();
        AlchemyForm view = new AlchemyForm();
        AlchemyPresenter presenter = new AlchemyPresenter(view, model);

        Application.Run(view);
    }
}