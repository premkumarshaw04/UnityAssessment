using UnityEngine;

namespace EndlessRunner.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "EndlessRunner/Configs/PlayerConfig")]
    public class PlayerConfigSO : ScriptableObject
    {
        [Header("Movement Settings")]
        [Tooltip("Forward movement running speed.")]
        [SerializeField, Range(5f, 30f)] private float forwardSpeed = 12f;

        [Tooltip("Speed multiplier during horizontal lane transitions.")]
        [SerializeField, Range(5f, 25f)] private float laneChangeSpeed = 15f;

        [Tooltip("Distance between adjacent tracks/lanes.")]
        [SerializeField, Range(1.5f, 4f)] private float laneWidth = 2.5f;

        [Header("Jump Settings")]
        [Tooltip("Initial upward force applied on jump.")]
        [SerializeField, Range(5f, 20f)] private float jumpForce = 10f;

        [Tooltip("Gravity multiplier applied when falling to make jumping feel responsive.")]
        [SerializeField, Range(1f, 5f)] private float gravityScale = 2.5f;

        [Header("Collision Layers")]
        [SerializeField] private LayerMask obstacleLayer;
        [SerializeField] private LayerMask coinLayer;

        public float ForwardSpeed => forwardSpeed;
        public float LaneChangeSpeed => laneChangeSpeed;
        public float LaneWidth => laneWidth;
        public float JumpForce => jumpForce;
        public float GravityScale => gravityScale;
        public LayerMask ObstacleLayer => obstacleLayer;
        public LayerMask CoinLayer => coinLayer;
    }
}