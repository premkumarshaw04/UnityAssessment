using UnityEngine;
using UnityEngine.InputSystem;
using EndlessRunner.Events;

namespace EndlessRunner.Core
{
    /// <summary>
    /// Touch swipe and keyboard input gesture processor.
    /// Uses Unity's New Input System API.
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        [Header("Swipe Parameters")]
        [SerializeField] private float minSwipeDistance = 50f;

        private Vector2 _touchStartPosition;
        private Vector2 _touchEndPosition;

        private void Update()
        {
            ProcessTouchInputs();
            ProcessKeyboardDebugInputs();
        }

        private void ProcessTouchInputs()
        {
            if (Touchscreen.current == null) return;

            var touch = Touchscreen.current.primaryTouch;

            if (touch.press.wasPressedThisFrame)
            {
                _touchStartPosition = touch.position.ReadValue();
            }

            if (touch.press.wasReleasedThisFrame)
            {
                _touchEndPosition = touch.position.ReadValue();
                EvaluateSwipe();
            }
        }

        private void ProcessKeyboardDebugInputs()
        {
            if (Keyboard.current == null) return;

            if (Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame)
            {
                EventBus<PlayerJumpedEvent>.Raise(new PlayerJumpedEvent());
            }
        }

        private void EvaluateSwipe()
        {
            Vector2 swipeDelta = _touchEndPosition - _touchStartPosition;

            if (swipeDelta.magnitude < minSwipeDistance) return;

            if (Mathf.Abs(swipeDelta.x) <= Mathf.Abs(swipeDelta.y))
            {
                // Vertical Swipe
                if (swipeDelta.y > 0)
                {
                    EventBus<PlayerJumpedEvent>.Raise(new PlayerJumpedEvent());
                }
            }
        }
    }
}