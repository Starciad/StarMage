using Microsoft.Xna.Framework.Input;

namespace StarMage.Core.InputSystem
{
    internal static class Controller
    {
        internal static bool IsMoveUpStarted()
        {
            return Input.Started(Keys.Up) || Input.Started(Buttons.DPadUp) || Input.Started(Buttons.LeftThumbstickUp);
        }

        internal static bool IsMoveUpPressed()
        {
            return Input.Performed(Keys.Up) || Input.Performed(Buttons.DPadUp) || Input.Performed(Buttons.LeftThumbstickUp);
        }

        internal static bool IsMoveDownStarted()
        {
            return Input.Started(Keys.Down) || Input.Started(Buttons.DPadDown) || Input.Started(Buttons.LeftThumbstickDown);
        }

        internal static bool IsMoveDownPressed()
        {
            return Input.Performed(Keys.Down) || Input.Performed(Buttons.DPadDown) || Input.Performed(Buttons.LeftThumbstickDown);
        }

        internal static bool IsMoveDownCanceled()
        {
            return Input.Canceled(Keys.Down) || Input.Canceled(Buttons.DPadDown) || Input.Canceled(Buttons.LeftThumbstickDown);
        }

        internal static bool IsMoveRightPressed()
        {
            return Input.Performed(Keys.Right) || Input.Performed(Buttons.DPadRight) || Input.Performed(Buttons.LeftThumbstickRight);
        }

        internal static bool IsMoveLeftPressed()
        {
            return Input.Performed(Keys.Left) || Input.Performed(Buttons.DPadLeft) || Input.Performed(Buttons.LeftThumbstickLeft);
        }

        internal static bool IsJumpStarted()
        {
            return Input.Started(Keys.Z) || Input.Started(Buttons.A);
        }

        internal static bool IsJumpCanceled()
        {
            return Input.Canceled(Keys.Z) || Input.Canceled(Buttons.A);
        }

        internal static bool IsRunPressed()
        {
            return Input.Performed(Keys.X) || Input.Performed(Buttons.X);
        }

        internal static bool IsShootStarted()
        {
            return Input.Started(Keys.X) || Input.Started(Buttons.X);
        }

        internal static bool IsStartStarted()
        {
            return Input.Started(Keys.Enter) || Input.Started(Buttons.Start);
        }

        internal static bool IsPauseStarted()
        {
            return Input.Started(Keys.Enter) || Input.Started(Buttons.Start);
        }
    }
}
