using StarMage.Core;

using System.Windows.Forms;

internal static class Program
{
    private static void Main()
    {
        _ = Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

        using StarMageGame game = new();
        game.Run();
    }
}