using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputAction _pressAction;
    [SerializeField] private InputAction _moveAction;
    [SerializeField] private float _dragSpeed = 0.1f;

    private Camera _mainCamera;
    private bool _isDragging = false;
    private Vector2 _velocity = Vector2.zero;

    private void OnEnable()
    {
        _mainCamera = Camera.main;

        _pressAction.Enable();
        _moveAction.Enable();

        _pressAction.performed += MousePressed;

        _pressAction.canceled += cancelDrag => { _isDragging = false; };
    }

    private void OnDisable()
    {
        _pressAction.performed -= MousePressed;
        _moveAction.Disable();
        _pressAction.Disable();
    }

    private void MousePressed(InputAction.CallbackContext context)
    {
        if (context.action.WasPressedThisFrame())
        {
            _isDragging = true;
            Vector2 initialPosition = _mainCamera.ScreenToWorldPoint(_moveAction.ReadValue<Vector2>());
            StartCoroutine(DragShip(initialPosition));
        }
    }

    private IEnumerator DragShip(Vector2 initialPos)
    {
        Vector2 currentPosition = transform.position;
        while (_isDragging)
        {
            Vector2 newPosition = _mainCamera.ScreenToWorldPoint(_moveAction.ReadValue<Vector2>());
            Vector2 finalPosition = newPosition - initialPos + currentPosition;
            transform.position = Vector2.SmoothDamp(transform.position, finalPosition, ref _velocity, _dragSpeed);
            yield return new WaitForFixedUpdate();
        }
    }
}