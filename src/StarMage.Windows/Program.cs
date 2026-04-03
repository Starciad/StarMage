using StarMage.Core;

using System.Windows.Forms;

namespace StarMage
{
    internal static class Program
    {
        private static void Main()
        {
            _ = Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

            using StarMageGame game = new();
            game.Run();
        }
    }
}
