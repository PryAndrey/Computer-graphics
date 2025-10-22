using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace MobiusStrip;
class Program
{
    static void Main(string[] args)
    {
        var nativeWindowSettings = new NativeWindowSettings()
        {
            ClientSize = new Vector2i(800, 600),
            Title = "Mobius Strip",
            
            Flags = ContextFlags.Default,
            APIVersion = new Version(3, 3),
            Profile = ContextProfile.Compatability
        };

        using var window = new ViewWindow(GameWindowSettings.Default, nativeWindowSettings);
        window.Run();
    }
}