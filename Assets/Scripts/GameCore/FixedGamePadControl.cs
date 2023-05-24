using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameCore.PlayerShips.Movement
{
    [DisallowMultipleComponent]
    public class FixedGamePadControl : MonoBehaviour
    {
        [Range(0, 100)] [SerializeField] float dragSpeed;

        [SerializeField] InputAction inputAction;

        RectTransform outerCircle, innerCircle;

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
            var currentShipPosition = transform.position;
            
            while (isDragging)
            {
                var newShipPosition = context.ReadValue<Vector2>().normalized;
                
                transform.position = new Vector2(
                    currentShipPosition.x + newShipPosition.x * dragSpeed * Time.deltaTime,
                    currentShipPosition.y + newShipPosition.y * dragSpeed * Time.deltaTime);

                currentShipPosition = transform.position;

                yield return new WaitForFixedUpdate();
            }
        }
    }
}