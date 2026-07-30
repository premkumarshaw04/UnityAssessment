using UnityEngine;
using EndlessRunner.Events;
using EndlessRunner.ScriptableObjects;

namespace EndlessRunner.Gameplay.Player
{
    /// <summary>
    /// Handles 3-lane movement mechanics, continuous forward progression,
    /// horizontal lane interpolation, and physics-driven jumping.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private PlayerConfigSO playerConfig;

        private CharacterController _characterController;
        private int _currentLane = 1; // 0 = Left, 1 = Center, 2 = Right
        private Vector3 _moveDirection;
        private float _verticalVelocity;
        private bool _isGrounded;
        private bool _isAlive = true;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
            EventBus<PlayerJumpedEvent>.Subscribe(OnJumpRequested);
            EventBus<GameStateChangedEvent>.Subscribe(OnGameStateChanged);
        }

        private void OnDisable()
        {
            EventBus<PlayerJumpedEvent>.Unsubscribe(OnJumpRequested);
            EventBus<GameStateChangedEvent>.Unsubscribe(OnGameStateChanged);
        }

        private void Update()
        {
            if (!_isAlive) return;

            ProcessGroundedState();
            ProcessHorizontalMovement();
            ProcessVerticalMovement();

            // Execute final move delta via CharacterController
            _characterController.Move(_moveDirection * Time.deltaTime);
        }

        /// <summary>
        /// Moves player laterally toward target lane coordinates.
        /// </summary>
        private void ProcessHorizontalMovement()
        {
            float targetX = (_currentLane - 1) * playerConfig.LaneWidth;
            Vector3 currentPos = transform.position;
            
            float xDelta = Mathf.MoveTowards(currentPos.x, targetX, playerConfig.LaneChangeSpeed * Time.deltaTime) - currentPos.x;

            _moveDirection.x = xDelta / Time.deltaTime;
            _moveDirection.z = playerConfig.ForwardSpeed;
        }

        /// <summary>
        /// Applies custom responsive gravity when airborne.
        /// </summary>
        private void ProcessVerticalMovement()
        {
            if (_isGrounded && _verticalVelocity < 0)
            {
                _verticalVelocity = -2f; // Keep grounded firmly
            }
            else
            {
                _verticalVelocity += Physics.gravity.y * playerConfig.GravityScale * Time.deltaTime;
            }

            _moveDirection.y = _verticalVelocity;
        }

        private void ProcessGroundedState()
        {
            _isGrounded = _characterController.isGrounded;
        }

        private void OnJumpRequested(PlayerJumpedEvent e)
        {
            if (_isGrounded && _isAlive)
            {
                _verticalVelocity = playerConfig.JumpForce;
            }
        }

        /// <summary>
        /// Shifts current lane indices safely between [0, 2].
        /// </summary>
        public void ChangeLane(int direction)
        {
            if (!_isAlive) return;
            _currentLane = Mathf.Clamp(_currentLane + direction, 0, 2);
        }

        private void OnGameStateChanged(GameStateChangedEvent e)
        {
            _isAlive = (e.CurrentState == GameState.Playing);
            
            if (e.CurrentState == GameState.Playing && e.PreviousState == GameState.MainMenu)
            {
                // Reset player position on game start
                _currentLane = 1;
                transform.position = Vector3.zero;
                _verticalVelocity = 0f;
            }
        }
    }
}