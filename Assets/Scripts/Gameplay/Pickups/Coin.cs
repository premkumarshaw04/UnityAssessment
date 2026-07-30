using UnityEngine;
using EndlessRunner.Interfaces;

namespace EndlessRunner.Gameplay.Pickups
{
    /// <summary>
    /// Collectible coin script supporting continuous rotational animation and pooling hooks.
    /// </summary>
    public class Coin : MonoBehaviour, IPoolable
    {
        [SerializeField] private float rotationSpeed = 100f;

        private void Update()
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        }

        public void OnSpawnFromPool()
        {
            gameObject.SetActive(true);
        }

        public void OnReturnToPool()
        {
            // Reset animation state
        }
    }
}