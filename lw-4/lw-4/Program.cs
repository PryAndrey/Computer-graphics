using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Cuboctahedron;

class Program
{
    static void Main(string[] args)
    {
        var nativeWindowSettings = new NativeWindowSettings()
        {
            ClientSize = new Vector2i(800, 600),
            Title = "Cuboctahedron",
            
            Flags = ContextFlags.Default,
            APIVersion = new Version(3, 3),
            Profile = ContextProfile.Compatability
        };

        using var window = new ViewWindow(GameWindowSettings.Default, nativeWindowSettings);
        window.Run();
    }
}