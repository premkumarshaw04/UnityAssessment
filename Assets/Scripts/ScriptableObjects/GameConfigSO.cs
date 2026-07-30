using UnityEngine;

namespace EndlessRunner.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "EndlessRunner/Configs/GameConfig")]
    public class GameConfigSO : ScriptableObject
    {
        [Header("Base Dynamics")]
        [Tooltip("Initial movement speed scaling multiplier.")]
        [SerializeField, Range(1f, 3f)] private float initialSpeedMultiplier = 1.0f;

        [Tooltip("Maximum allowed speed multiplier.")]
        [SerializeField, Range(2f, 5f)] private float maxSpeedMultiplier = 3.0f;

        [Tooltip("Rate at which speed increases per second.")]
        [SerializeField, Range(0.001f, 0.05f)] private float speedIncreaseRate = 0.01f;

        [Header("Scoring Metrics")]
        [Tooltip("Points awarded per meter traveled.")]
        [SerializeField] private int pointsPerMeter = 10;

        [Tooltip("Points awarded per collected coin.")]
        [SerializeField] private int pointsPerCoin = 50;

        public float InitialSpeedMultiplier => initialSpeedMultiplier;
        public float MaxSpeedMultiplier => maxSpeedMultiplier;
        public float SpeedIncreaseRate => speedIncreaseRate;
        public int PointsPerMeter => pointsPerMeter;
        public int PointsPerCoin => pointsPerCoin;
    }
}