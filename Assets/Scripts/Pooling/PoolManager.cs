using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using EndlessRunner.Interfaces;

namespace EndlessRunner.Pooling
{
    /// <summary>
    /// Centralized service handling object pool creation, retrieval, and recycling.
    /// Uses InstanceID integer lookups for zero-allocation performance.
    /// </summary>
    public class PoolManager : MonoBehaviour, IGameService
    {
        private readonly Dictionary<int, GameObjectPool> _pools = new Dictionary<int, GameObjectPool>();
        private readonly Dictionary<int, int> _instanceToPrefabMap = new Dictionary<int, int>();

        public Task InitializeAsync()
        {
            Debug.Log("[PoolManager] Initialized successfully.");
            return Task.CompletedTask;
        }

        public void Deinitialize()
        {
            _pools.Clear();
            _instanceToPrefabMap.Clear();
        }

        /// <summary>
        /// Pre-warms an object pool for a specific prefab asset.
        /// </summary>
        public void CreatePool(GameObject prefab, int initialCapacity)
        {
            if (prefab == null) return;

            int prefabId = prefab.GetInstanceID();
            if (!_pools.ContainsKey(prefabId))
            {
                GameObject poolContainer = new GameObject($"Pool_{prefab.name}");
                poolContainer.transform.SetParent(transform);

                GameObjectPool pool = new GameObjectPool(prefab, initialCapacity, poolContainer.transform);
                _pools.Add(prefabId, pool);
            }
        }

        /// <summary>
        /// Spawns an instance from the designated pool at specified transform coordinates.
        /// </summary>
        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            if (prefab == null) return null;

            int prefabId = prefab.GetInstanceID();
            if (!_pools.ContainsKey(prefabId))
            {
                CreatePool(prefab, 5); // Fallback pre-warm capacity
            }

            GameObject instance = _pools[prefabId].Get(position, rotation);
            _instanceToPrefabMap[instance.GetInstanceID()] = prefabId;

            return instance;
        }

        /// <summary>
        /// Recycles a spawned GameObject back to its assigned parent pool stack.
        /// </summary>
        public void Despawn(GameObject instance)
        {
            if (instance == null) return;

            int instanceId = instance.GetInstanceID();
            if (_instanceToPrefabMap.TryGetValue(instanceId, out int prefabId))
            {
                if (_pools.TryGetValue(prefabId, out GameObjectPool pool))
                {
                    pool.Release(instance);
                    _instanceToPrefabMap.Remove(instanceId);
                    return;
                }
            }

            Debug.LogWarning($"[PoolManager] Attempted to despawn unmanaged object {instance.name}. Destroying fallback.");
            Destroy(instance);
        }
    }
}