using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Tetris
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var nativeWindowSettings = new NativeWindowSettings
            {
                ClientSize = new Vector2i(600, 600),
                MinimumClientSize = new Vector2i(600, 600),
                MaximumClientSize = new Vector2i(900, 1000),
                Location = new Vector2i(370, 300),
                WindowBorder = WindowBorder.Resizable,
                WindowState = WindowState.Normal,
                Title = "Tetris",

                Flags = ContextFlags.Default,
                APIVersion = new Version(3, 3),
                Profile = ContextProfile.Compatability
            };

            using (ViewWindow game = new ViewWindow(GameWindowSettings.Default, nativeWindowSettings))
            {
                game.Run();
            }
        }
    }
}