using UnityEngine.InputSystem;
using System.Collections;
using Store;
using UnityEngine;

namespace GameCore.PlayerShips.Movement
{
    [DisallowMultipleComponent]
    public class TouchControl : MonoBehaviour
    {

        Vector2 velocity = Vector2.zero;

        [Range(0, 1)]
        [SerializeField] float dragSpeed;

        [SerializeField] InputAction inputAction;

        void Awake()
        {
            GameSettings.controlType.subscribe += v => enabled = v == GameSettings.ControlType.ScreenDrag;
        }

        bool isDragging;

        void OnEnable()
        {
            inputAction.Enable();
            inputAction.performed += InputPerformed;
            inputAction.canceled += _ =>
            {
                isDragging = false;
                parentRestrictedMovement.desiredPosition = PlayerShip.instance.transform.position;
            };
        }

        void OnDisable()
        {
            inputAction.performed -= InputPerformed;
            inputAction.Disable();
        }

        RestrictedMovement _parentRestrictedMovement;
        RestrictedMovement parentRestrictedMovement => _parentRestrictedMovement = _parentRestrictedMovement
            ? _parentRestrictedMovement
            : this.GetLinkedInParents<RestrictedMovement>();

        void InputPerformed(InputAction.CallbackContext context)
        {
            if (!context.action.WasPressedThisFrame()) return;

            isDragging = true;
            parentRestrictedMovement.desiredPosition = PlayerShip.instance.transform.position;
            StartCoroutine(DragShip(context));
        }

        IEnumerator DragShip(InputAction.CallbackContext context)
        {
            Vector2 initialPosition = GameCamera.instance.linkedCamera.ScreenToWorldPoint(context.ReadValue<Vector2>());
            var currentPosition = parentRestrictedMovement.desiredPosition;
            while (isDragging)
            {
                Vector2 newPosition = GameCamera.instance.linkedCamera.ScreenToWorldPoint(context.ReadValue<Vector2>());
                Vector2 finalPosition = newPosition - initialPosition + currentPosition;
                // PlayerShip.instance.transform.position = Vector2.SmoothDamp(PlayerShip.instance.transform.position,
                //     finalPosition, ref velocity, dragSpeed);
                parentRestrictedMovement.desiredPosition = Vector2.SmoothDamp(parentRestrictedMovement.desiredPosition, finalPosition, ref velocity, dragSpeed);
                // parentRestrictedMovement.desiredPosition = finalPosition;
                yield return new WaitForFixedUpdate();
            }
        }
    }
}