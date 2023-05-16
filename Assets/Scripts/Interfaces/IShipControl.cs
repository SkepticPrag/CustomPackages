using UnityEngine;

public interface IShipControl : IToggler
{
    public void EnableShip(IInputControl inputControl);

    public GameObject GetShipGameObject();
}
