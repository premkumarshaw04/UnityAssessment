using UnityEngine;
using EndlessRunner.Interfaces;

namespace EndlessRunner.Gameplay.Environment
{
    /// <summary>
    /// Represents an individual pooled track segment.
    /// Manages local spawn points for obstacles and coins.
    /// </summary>
    public class GroundTile : MonoBehaviour, IPoolable
    {
        [Header("Spawn Markers")]
        [SerializeField] private Transform[] obstacleSpawnPoints;
        [SerializeField] private Transform[] coinSpawnPoints;
        [SerializeField] private Transform nextTileAttachPoint;

        public Vector3 NextTilePosition => nextTileAttachPoint != null ? nextTileAttachPoint.position : transform.position + new Vector3(0, 0, 20f);
        public Transform[] ObstacleSpawnPoints => obstacleSpawnPoints;
        public Transform[] CoinSpawnPoints => coinSpawnPoints;

        public void OnSpawnFromPool()
        {
            // Reset tile state when activated
        }

        public void OnReturnToPool()
        {
            // Clean up child objects or flags when recycled
        }
    }
}