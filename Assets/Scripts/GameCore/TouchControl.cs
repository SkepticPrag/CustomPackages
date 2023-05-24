using UnityEngine.InputSystem;
using System.Collections;
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
        

        bool isDragging;

        void OnEnable()
        {
            inputAction.Enable();
            inputAction.performed += InputPerformed;
            inputAction.canceled += _ =>
            {
                isDragging = false;
            };
        }

        void OnDisable()
        {
            inputAction.performed -= InputPerformed;
            inputAction.Disable();
        }

        void InputPerformed(InputAction.CallbackContext context)
        {
            if (!context.action.WasPressedThisFrame()) return;

            isDragging = true;
            StartCoroutine(DragShip(context));
        }

        IEnumerator DragShip(InputAction.CallbackContext context)
        {
            Vector2 initialPosition = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
            Vector2 currentPosition = transform.position;
            while (isDragging)
            {
                Vector2 newPosition = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
                Vector2 finalPosition = newPosition - initialPosition + currentPosition;
                // PlayerShip.instance.transform.position = Vector2.SmoothDamp(PlayerShip.instance.transform.position,
                //     finalPosition, ref velocity, dragSpeed);
                transform.position = Vector2.SmoothDamp(transform.position, finalPosition, ref velocity, dragSpeed);
                // parentRestrictedMovement.desiredPosition = finalPosition;
                yield return new WaitForFixedUpdate();
            }
        }
    }
}