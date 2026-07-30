using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using EndlessRunner.Interfaces;

namespace EndlessRunner.Pooling
{
    /// <summary>
    /// Central manager for all object pools.
    /// Unity 6.5 compatible implementation.
    /// </summary>
    public class PoolManager : MonoBehaviour, IGameService
    {
        private readonly Dictionary<GameObject, GameObjectPool> _pools = new();
        private readonly Dictionary<GameObject, GameObject> _spawnedObjects = new();

        public Task InitializeAsync()
        {
            Debug.Log("[PoolManager] Initialized.");
            return Task.CompletedTask;
        }

        public void Deinitialize()
        {
            _pools.Clear();
            _spawnedObjects.Clear();
        }

        /// <summary>
        /// Creates a pool for a prefab if one does not already exist.
        /// </summary>
        public void CreatePool(GameObject prefab, int initialSize)
        {
            if (prefab == null)
                return;

            if (_pools.ContainsKey(prefab))
                return;

            GameObject container = new GameObject($"Pool_{prefab.name}");
            container.transform.SetParent(transform);

            GameObjectPool pool =
                new GameObjectPool(prefab, initialSize, container.transform);

            _pools.Add(prefab, pool);
        }

        /// <summary>
        /// Spawns an object from its pool.
        /// </summary>
        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            if (prefab == null)
                return null;

            if (!_pools.TryGetValue(prefab, out GameObjectPool pool))
            {
                CreatePool(prefab, 5);
                pool = _pools[prefab];
            }

            GameObject instance = pool.Get(position, rotation);

            if (!_spawnedObjects.ContainsKey(instance))
                _spawnedObjects.Add(instance, prefab);

            return instance;
        }

        /// <summary>
        /// Returns an object back to its original pool.
        /// </summary>
        public void Despawn(GameObject instance)
        {
            if (instance == null)
                return;

            if (_spawnedObjects.TryGetValue(instance, out GameObject prefab))
            {
                _pools[prefab].Release(instance);
                _spawnedObjects.Remove(instance);
            }
            else
            {
                Debug.LogWarning($"PoolManager: {instance.name} was not created from a pool.");
                Destroy(instance);
            }
        }
    }
}