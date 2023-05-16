using UnityEngine;

public class InputControlInstaller : MonoBehaviour
{
    private IShipControl _ship;
    private IInputControl _touchControl;
    private IInputControl _joystickControl;
    private InputControlSwitcher _inputControlSwitcher;

    private void OnEnable()
    {
        _ship = GetComponentInChildren<IShipControl>();
        _touchControl = GetComponentInChildren<TouchControl>();
        _joystickControl = GetComponentInChildren<JoystickControl>();
        _inputControlSwitcher = GetComponent<InputControlSwitcher>();

        _touchControl?.SetShipGameObject(_ship.GetShipGameObject());
        _joystickControl?.SetShipGameObject(_ship.GetShipGameObject());

        IInputControl controlInUse = _inputControlSwitcher?.GetCurrentControl(_touchControl, _joystickControl);

        _ship.EnableShip(controlInUse);
    }
}