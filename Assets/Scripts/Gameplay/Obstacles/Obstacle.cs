using UnityEngine;
using EndlessRunner.Interfaces;

namespace EndlessRunner.Gameplay.Obstacles
{
    /// <summary>
    /// Individual pooled obstacle instance script.
    /// Implements IPoolable to handle resetting logic upon recycling.
    /// </summary>
    public class Obstacle : MonoBehaviour, IPoolable
    {
        public void OnSpawnFromPool()
        {
            // Ensure obstacle hitboxes and mesh components are active
        }

        public void OnReturnToPool()
        {
            // Reset animations or transform flags
        }
    }
}