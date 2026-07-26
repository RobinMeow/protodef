using System;
using Godot;

public static class InputEventExtensions
{
    extension(InputEvent inputEvent)
    {
        /// <summary>
        /// Checks if the input event corresponds to a specific mouse button press, along with any required modifier keys.
        /// </summary>
        /// <param name="btn">The <see cref="MouseButton"/> that needs to be pressed.</param>
        /// <param name="modifiers_pressed">
        /// An optional, variable-length array of modifier keys that must be held down simultaneously.
        /// Possible values are <c>"shift"</c>, <c>"ctrl"</c>, <c>"alt"</c>, and <c>"meta"</c>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the event is a mouse button press matching the specified <paramref name="btn"/> and all provided <paramref name="modifiers_pressed"/>; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// It is recommended to pass string literals directly (e.g., <c>"shift"</c>, <c>"ctrl"</c>) for the <paramref name="modifiers_pressed"/> arguments instead of using named constants.
        /// </remarks>
        public bool IsPressed(MouseButton btn, params string[] modifiers_pressed)
        {
            if (!inputEvent.IsMouseEvent(out InputEventMouseButton mouse_event))
                return false;

            bool btn_pressed = mouse_event.Pressed && mouse_event.ButtonIndex == btn;

            if (!btn_pressed)
                return false;

            for (int i = 0; i < modifiers_pressed.Length; i++)
            {
                string modifier = modifiers_pressed[i];
                if (!IsPressingModifier(mouse_event, modifier))
                    return false;
            }

            return true;
        }

        public bool IsMouseEvent(out InputEventMouseButton mouseEvent)
        {
            if (inputEvent is InputEventMouseButton mouse_event)
            {
                mouseEvent = mouse_event;
                return true;
            }
            else
            {
                mouseEvent = null!;
                return false;
            }
        }
    }

    extension(InputEventMouseButton mouseButton)
    {
        /// <summary>
        /// Checks if a specific modifier key was held down during the mouse event.
        /// </summary>
        /// <param name="modifier">
        /// The name of the modifier key to check. Possible values are <c>"shift"</c>, <c>"ctrl"</c>, <c>"alt"</c>, and <c>"meta"</c>.
        /// </param>
        /// <returns><c>true</c> if the specified modifier key is pressed; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// It is recommended to use string literals directly (e.g., <c>"shift"</c>) instead of named constants when calling this method.
        /// Possible values are <c>"shift"</c>, <c>"ctrl"</c>, <c>"alt"</c>, and <c>"meta"</c>.
        /// </remarks>
        /// <returns><c>true</c> if the specified modifier key is pressed; otherwise, <c>false</c>.</returns>
        public bool IsPressingModifier(string modifier)
        {
            switch (modifier)
            {
                case "shift":
                    return mouseButton.ShiftPressed;
                case "ctrl":
                    return mouseButton.CtrlPressed;
                case "alt":
                    return mouseButton.AltPressed;
                case "meta":
                    return mouseButton.MetaPressed;
                default:
                    throw new InvalidOperationException("Should never reach.");
            }
        }
    }
}
