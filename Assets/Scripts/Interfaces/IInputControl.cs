using UnityEngine;
using UnityEngine.InputSystem;

public interface IInputControl : IToggler
{
    GameObject GetInputControlGameObject();

    void SetShipGameObject(GameObject ship);

    void InputPerformed(InputAction.CallbackContext context);
}
