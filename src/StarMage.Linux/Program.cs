using StarMage.Core;

namespace StarMage
{
    internal static class Program
    {
        private static void Main()
        {
            using StarMageGame game = new();
            game.Run();
        }
    }
}
