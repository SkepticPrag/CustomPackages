using UnityEngine;

public class ShipControl : MonoBehaviour, IShipControl
{
    private IInputControl _inputControl;

    public void EnableShip(IInputControl inputControl)
    {
        _inputControl = inputControl;

        Activate();
    }

    public void Activate()
    {
        _inputControl?.Activate();
    }

    public void Terminate()
    {
        _inputControl?.Terminate();
    }

    public GameObject GetShipGameObject()
    {
        return transform.gameObject;
    }
}