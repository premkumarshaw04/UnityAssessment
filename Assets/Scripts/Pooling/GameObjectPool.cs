using System.Collections.Generic;
using UnityEngine;
using EndlessRunner.Interfaces;

namespace EndlessRunner.Pooling
{
    /// <summary>
    /// Generic object pool managing active instances of a specific prefab asset.
    /// </summary>
    public class GameObjectPool
    {
        private readonly GameObject _prefab;
        private readonly Transform _parentContainer;
        private readonly Stack<GameObject> _poolStack;

        public GameObject Prefab => _prefab;

        public GameObjectPool(GameObject prefab, int initialCapacity, Transform parentContainer)
        {
            _prefab = prefab;
            _parentContainer = parentContainer;
            _poolStack = new Stack<GameObject>(initialCapacity);

            Prewarm(initialCapacity);
        }

        private void Prewarm(int count)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject instance = Object.Instantiate(_prefab, _parentContainer);
                instance.SetActive(false);
                _poolStack.Push(instance);
            }
        }

        /// <summary>
        /// Gets an inactive object from the pool stack or expands dynamically if empty.
        /// </summary>
        public GameObject Get(Vector3 position, Quaternion rotation)
        {
            GameObject instance;

            if (_poolStack.Count > 0)
            {
                instance = _poolStack.Pop();
            }
            else
            {
                Debug.LogWarning($"[GameObjectPool] Pool for {_prefab.name} exhausted. Expanding pool dynamically.");
                instance = Object.Instantiate(_prefab, _parentContainer);
            }

            instance.transform.SetPositionAndRotation(position, rotation);
            instance.SetActive(true);

            if (instance.TryGetComponent(out IPoolable poolable))
            {
                poolable.OnSpawnFromPool();
            }

            return instance;
        }

        /// <summary>
        /// Returns an active instance back to the pool stack.
        /// </summary>
        public void Release(GameObject instance)
        {
            if (instance.TryGetComponent(out IPoolable poolable))
            {
                poolable.OnReturnToPool();
            }

            instance.SetActive(false);
            instance.transform.SetParent(_parentContainer);
            _poolStack.Push(instance);
        }
    }
}