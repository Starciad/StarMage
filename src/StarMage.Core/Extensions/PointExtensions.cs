using Microsoft.Xna.Framework;

using System;

namespace StarMage.Core.Extensions
{
    internal static class PointExtensions
    {
        internal static float DistanceTo(this Point a, Point b)
        {
            int deltaX = b.X - a.X;
            int deltaY = b.Y - a.Y;
            return MathF.Sqrt((deltaX * deltaX) + (deltaY * deltaY));
        }
    }
}
