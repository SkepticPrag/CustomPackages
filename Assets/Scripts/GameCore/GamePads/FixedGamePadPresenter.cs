using Store;
using UnityEngine;

namespace GameCore.PlayerShips.Movement.GamePads
{
    [DisallowMultipleComponent]
    public class FixedGamePadPresenter : MonoBehaviourSingleton<FixedGamePadPresenter>
    {
        protected override void Awake()
        {
            base.Awake();
            GameSettings.controlType.subscribe += v => gameObject.SetActive(v == GameSettings.ControlType.FixedJoystick);
        }
    }
}