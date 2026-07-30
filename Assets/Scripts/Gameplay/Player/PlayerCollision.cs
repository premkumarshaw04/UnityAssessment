using UnityEngine;
using EndlessRunner.Core;
using EndlessRunner.Events;
using EndlessRunner.ScriptableObjects;

namespace EndlessRunner.Gameplay.Player
{
    /// <summary>
    /// Processes physical trigger contacts with Coins and Obstacles.
    /// Publishes zero-allocation event payloads to the EventBus.
    /// </summary>
    public class PlayerCollision : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private PlayerConfigSO playerConfig;

        [Header("Dependencies")]
        [SerializeField] private GameManager gameManager;

        private void OnTriggerEnter(Collider other)
        {
            int layerMask = 1 << other.gameObject.layer;

            // Handle Obstacle Collision
            if ((layerMask & playerConfig.ObstacleLayer) != 0)
            {
                gameManager.TriggerGameOver("Obstacle Hit");
                return;
            }

            // Handle Coin Collection
            if ((layerMask & playerConfig.CoinLayer) != 0)
            {
                EventBus<CoinCollectedEvent>.Raise(new CoinCollectedEvent(1, other.transform.position));
                other.gameObject.SetActive(false); // Triggers pooled return hook
            }
        }
    }
}