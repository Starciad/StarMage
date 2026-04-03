using Microsoft.Xna.Framework;

namespace StarMage.Core.Handlers
{
    internal static class CameraHandler
    {
        internal static Point Position { get; set; }

        private static Matrix GetVirtualViewMatrix()
        {
            return Matrix.CreateTranslation(new(-Position.X, -Position.Y, 0.0f)) *
                   Matrix.CreateScale(1.0f, 1.0f, 1.0f);
        }

        internal static Matrix GetViewMatrix()
        {
            return GetVirtualViewMatrix() * Matrix.Identity;
        }

        internal static Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, GetViewMatrix());
        }

        internal static Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, Matrix.Invert(GetViewMatrix()));
        }

        internal static Rectangle GetViewBounds()
        {
            Point viewportSize = Constants.SCREEN_SIZE;

            Point topLeft = ScreenToWorld(Vector2.Zero).ToPoint();
            Point bottomRight = ScreenToWorld(viewportSize.ToVector2()).ToPoint();

            int width = bottomRight.X - topLeft.X;
            int height = bottomRight.Y - topLeft.Y;

            return new(topLeft.X, topLeft.Y, width, height);
        }
    }
}
