using UnityEngine;

public class InputControlSwitcher : MonoBehaviour
{
    [SerializeField] private bool _isUsingJoystick;

    [HideInInspector]
    [SerializeField] private bool _isJoystickFixed;

    public IInputControl GetCurrentControl(IInputControl touchControl, IInputControl joystickControl)
    {
        if (!_isUsingJoystick)
        {
            joystickControl.Terminate();
            return touchControl;
        }
        touchControl.Terminate();
        return joystickControl;
    }
}
