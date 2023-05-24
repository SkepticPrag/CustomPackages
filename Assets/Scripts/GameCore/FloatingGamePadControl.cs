using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameCore.PlayerShips.Movement
{
    [DisallowMultipleComponent]
    public class FloatingGamePadControl : MonoBehaviour
    {
        [Range(0, 100)] [SerializeField] float dragSpeed;

        [SerializeField] InputAction inputTouchAction;
        [SerializeField] InputAction inputMoveAction;

        Vector2 joystickHandleInitialPosition, joystickPadInitialPosition;

        RectTransform outerCircle, innerCircle;

        Canvas myCanvas;

        Vector2 joystickHandleInitialPos, joystickPadInitialPos;

        bool isDragging;
        

        void OnEnable()
        {
            // myCanvas = FloatingGamePadPresenter.instance.transform.root.GetComponent<Canvas>();
            // outerCircle = FloatingGamePadPresenter.instance.transform.GetComponent<RectTransform>();
            // innerCircle = FloatingGamePadPresenter.instance.transform.GetChild(0).GetComponent<RectTransform>();

            joystickPadInitialPos = outerCircle.position;
            joystickHandleInitialPos = innerCircle.position;

            inputTouchAction.Enable();
            inputTouchAction.performed += InputTouchPerformed;
            inputTouchAction.canceled += _ =>
            {
                isDragging = false;
                outerCircle.position = joystickPadInitialPos;
                innerCircle.position = joystickHandleInitialPos;
            };

            inputMoveAction.Enable();
            inputMoveAction.performed += InputMovePerformed;
        }

        void OnDisable()
        {
            inputTouchAction.performed -= InputTouchPerformed;
            inputMoveAction.performed -= InputMovePerformed;

            inputTouchAction.Disable();
            inputMoveAction.Disable();
        }

        void InputTouchPerformed(InputAction.CallbackContext context)
        {
            if (!context.action.WasPressedThisFrame()) return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)myCanvas.transform,
                context.ReadValue<Vector2>(), null, out var initialPosition);

            outerCircle.transform.position = myCanvas.transform.TransformPoint(initialPosition);
            innerCircle.transform.position = myCanvas.transform.TransformPoint(initialPosition);
        }

        void InputMovePerformed(InputAction.CallbackContext context)
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
                var newShipPosition = context.ReadValue<Vector2>();

                transform.position = new Vector2(
                    currentShipPosition.x + newShipPosition.x * dragSpeed * Time.deltaTime,
                    currentShipPosition.y + newShipPosition.y * dragSpeed * Time.deltaTime);

                currentShipPosition = transform.position;

                yield return new WaitForFixedUpdate();
            }
        }
    }
}