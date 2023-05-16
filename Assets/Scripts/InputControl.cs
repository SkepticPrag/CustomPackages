using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;

public abstract class InputControl : MonoBehaviour, IInputControl
{
    [SerializeField] private InputAction _inputAction;

    protected Camera _mainCamera;
    protected bool _isDragging = false;

    protected Vector2 _velocity = Vector2.zero;
    protected GameObject _shipGameObject;

    public void Activate()
    {
        _mainCamera = Camera.main;

        _inputAction.Enable();
        _inputAction.performed += InputPerformed;
        _inputAction.canceled += cancelDrag => { _isDragging = false; };
    }

    public void Terminate()
    {
        _inputAction.performed -= InputPerformed;
        _inputAction.Disable();
        gameObject.SetActive(false);
    }

    public void InputPerformed(InputAction.CallbackContext context)
    {
        if (context.action.WasPressedThisFrame())
        {
             _isDragging = true;
            StartCoroutine(DragShip(context));
        }
    }

    public void SetShipGameObject(GameObject inputControlGameObject)
    {
        _shipGameObject = inputControlGameObject;
    }

    public GameObject GetInputControlGameObject()
    {
        return transform.gameObject;
    }

    public abstract IEnumerator DragShip(InputAction.CallbackContext context);

}
