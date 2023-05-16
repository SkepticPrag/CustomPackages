using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;

public class JoystickControl : InputControl
{
    [Range(0, 100)]
    [SerializeField] private float _dragSpeed;

    [SerializeField] private bool _isJoystickFixedInPlace;

    [SerializeField] private float _clampMagnitude = 5f;

    [SerializeField] private Transform _outerCircle;
    [SerializeField] private Transform _innerCircle;

    private Vector2 _gameObjectOriginPoint;

    private void OnEnable()
    {
        _gameObjectOriginPoint = transform.position;
    }

    public override IEnumerator DragShip(InputAction.CallbackContext context)
    {
        Vector2 staticJoystickPosition = _gameObjectOriginPoint;
        Vector2 dinamicJoystickinitialPosition = _mainCamera.ScreenToWorldPoint(context.ReadValue<Vector2>());

        if (_isJoystickFixedInPlace)
        {
            _outerCircle.position = staticJoystickPosition;
            _innerCircle.position = staticJoystickPosition;
        }
        else
        {
            _outerCircle.position = dinamicJoystickinitialPosition;
            _innerCircle.position = dinamicJoystickinitialPosition;
        }

        Vector2 currentShipPosition = _shipGameObject.transform.position;

        while (_isDragging)
        {
            Vector2 newJoystickPosition = _mainCamera.ScreenToWorldPoint(context.ReadValue<Vector2>());

            if (!_isJoystickFixedInPlace)
            {
                Vector2 offset = newJoystickPosition - dinamicJoystickinitialPosition;

                Vector2 direction = Vector2.ClampMagnitude(offset, _clampMagnitude);

                _innerCircle.position = new Vector2(dinamicJoystickinitialPosition.x + direction.x, dinamicJoystickinitialPosition.y + direction.y);
            }
            else
            {
                Vector2 offset = newJoystickPosition - dinamicJoystickinitialPosition;

                Vector2 direction = Vector2.ClampMagnitude(offset, _clampMagnitude);

                _innerCircle.position = new Vector2(staticJoystickPosition.x + direction.x, staticJoystickPosition.y + direction.y);
            }

            Vector2 newShipPosition = (newJoystickPosition - dinamicJoystickinitialPosition).normalized;

            _shipGameObject.transform.position = new Vector2(currentShipPosition.x + newShipPosition.x * _dragSpeed * Time.deltaTime,
                                             currentShipPosition.y + newShipPosition.y * _dragSpeed * Time.deltaTime);

            currentShipPosition = _shipGameObject.transform.position;

            yield return new WaitForFixedUpdate();
        }

        _outerCircle.position = staticJoystickPosition;
        _innerCircle.position = staticJoystickPosition;
    }
}
