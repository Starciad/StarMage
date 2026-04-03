using StarMage.Core.Databases;
using StarMage.Core.Enums;

namespace StarMage.Core.Handlers
{
    internal static class PaletteHandler
    {
        internal static void SetPalette(PaletteIndex index)
        {
            AssetDatabase.GetEffect(EffectIndex.PaletteIndexed).Parameters["Palette"].SetValue(PaletteDatabase.GetPalette(index).Vectors);
        }
    }
}
