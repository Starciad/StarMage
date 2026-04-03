using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace StarMage.Core.InputSystem
{
    internal static class Input
    {
        internal static KeyboardState CurrentKeyboardState { get; private set; }
        internal static KeyboardState PreviousKeyboardState { get; private set; }

        internal static GamePadState CurrentGamePadState { get; private set; }
        internal static GamePadState PreviousGamePadState { get; private set; }

        internal static void Update()
        {
            PreviousKeyboardState = CurrentKeyboardState;
            PreviousGamePadState = CurrentGamePadState;

            CurrentKeyboardState = Keyboard.GetState();
            CurrentGamePadState = GamePad.GetState(PlayerIndex.One);
        }

        #region KEYBOARD

        internal static bool Started(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key) &&
                   !PreviousKeyboardState.IsKeyDown(key);
        }

        internal static bool Performed(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key) &&
                   PreviousKeyboardState.IsKeyDown(key);
        }

        internal static bool Canceled(Keys key)
        {
            return !CurrentKeyboardState.IsKeyDown(key) &&
                   PreviousKeyboardState.IsKeyDown(key);
        }

        #endregion

        #region GAMEPAD

        internal static bool Started(Buttons button)
        {
            return CurrentGamePadState.IsButtonDown(button) &&
                   !PreviousGamePadState.IsButtonDown(button);
        }

        internal static bool Performed(Buttons button)
        {
            return CurrentGamePadState.IsButtonDown(button) &&
                   PreviousGamePadState.IsButtonDown(button);
        }

        internal static bool Canceled(Buttons button)
        {
            return !CurrentGamePadState.IsButtonDown(button) &&
                   PreviousGamePadState.IsButtonDown(button);
        }

        #endregion
    }
}
