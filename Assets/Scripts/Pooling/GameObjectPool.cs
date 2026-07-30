using System.Collections.Generic;
using UnityEngine;

namespace EndlessRunner.Pooling
{
    /// <summary>
    /// Simple GameObject pool compatible with Unity 6.5.
    /// </summary>
    public class GameObjectPool
    {
        private readonly GameObject _prefab;
        private readonly Transform _parent;
        private readonly Queue<GameObject> _pool = new();

        public GameObjectPool(GameObject prefab, int initialSize, Transform parent)
        {
            _prefab = prefab;
            _parent = parent;

            for (int i = 0; i < initialSize; i++)
            {
                GameObject obj = Object.Instantiate(_prefab, _parent);
                obj.SetActive(false);
                _pool.Enqueue(obj);
            }
        }

        public GameObject Get(Vector3 position, Quaternion rotation)
        {
            GameObject obj;

            if (_pool.Count > 0)
            {
                obj = _pool.Dequeue();
            }
            else
            {
                obj = Object.Instantiate(_prefab, _parent);
            }

            obj.transform.SetPositionAndRotation(position, rotation);
            obj.SetActive(true);

            return obj;
        }

        public void Release(GameObject obj)
        {
            obj.SetActive(false);
            obj.transform.SetParent(_parent);
            _pool.Enqueue(obj);
        }
    }
}