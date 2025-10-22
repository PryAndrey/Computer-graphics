using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

class Program
{
    static void Main(string[] args)
    {
        var nativeWinSettings = new NativeWindowSettings()
        {
            ClientSize = new Vector2i(1200, 800),
            Title = "Mobius Strip",

            Flags = ContextFlags.Default,
            APIVersion = new Version(3, 3),
            Profile = ContextProfile.Compatability
        };

        ViewWindow game = new(GameWindowSettings.Default, nativeWinSettings);
        game.Run();
    }
}