using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;

public class TouchControl : InputControl
{
    [Range(0, 1)]
    [SerializeField] private float _dragSpeed;

    public override IEnumerator DragShip(InputAction.CallbackContext context)
    {
        Vector2 initialPosition = _mainCamera.ScreenToWorldPoint(context.ReadValue<Vector2>());
        Vector2 currentPosition = _shipGameObject.transform.position;
        while (_isDragging)
        {
            Vector2 newPosition = _mainCamera.ScreenToWorldPoint(context.ReadValue<Vector2>());
            Vector2 finalPosition = newPosition - initialPosition + currentPosition;
            _shipGameObject.transform.position = Vector2.SmoothDamp(_shipGameObject.transform.position, finalPosition, ref _velocity, _dragSpeed);
            yield return new WaitForFixedUpdate();
        }
    }
}
