using System.Collections;
using Store;
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

        void Awake()
        {
            GameSettings.controlType.subscribe += v => enabled = v == GameSettings.ControlType.FixedJoystick;
        }

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

        void InputPerformed(InputAction.CallbackContext context)
        {
            if (!context.action.WasPressedThisFrame()) return;

            isDragging = true;
            parentRestrictedMovement.desiredPosition = PlayerShip.instance.transform.position;
            StartCoroutine(DragShip(context));
        }

        IEnumerator DragShip(InputAction.CallbackContext context)
        {
            var currentShipPosition = parentRestrictedMovement.desiredPosition;
            
            while (isDragging)
            {
                var newShipPosition = context.ReadValue<Vector2>().normalized;
                
                parentRestrictedMovement.desiredPosition = new Vector2(
                    currentShipPosition.x + newShipPosition.x * dragSpeed * Time.deltaTime,
                    currentShipPosition.y + newShipPosition.y * dragSpeed * Time.deltaTime);

                currentShipPosition = parentRestrictedMovement.desiredPosition;

                yield return new WaitForFixedUpdate();
            }
        }
    }
}