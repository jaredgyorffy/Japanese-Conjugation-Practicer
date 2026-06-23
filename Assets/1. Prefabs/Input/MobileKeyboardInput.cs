using UnityEngine;

public static class MobileKeyboardInput
{
    public static TouchScreenKeyboard.Status CheckInput()
    {
        if (TouchScreenKeyboard.visible == false)
        {
            return TouchScreenKeyboard.Status.LostFocus;
        }

        TouchScreenKeyboard touchScreenKeyboard = TouchScreenKeyboard.Open("");
        if (touchScreenKeyboard != null)
        {
            return touchScreenKeyboard.status;
        }
        else
        {
            return TouchScreenKeyboard.Status.LostFocus;
        }
    }
}