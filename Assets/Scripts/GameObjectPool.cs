using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids
{
    public class GameObjectPool
    {
        private readonly Transform _utilizationContainer;

        private readonly List<Tuple<GameObject, GameObject>> _pool = new();

        public GameObjectPool(Transform utilizationContainer = null)
        {
            _utilizationContainer = utilizationContainer != null
                ? utilizationContainer
                : new GameObject(nameof(GetType)).transform;
            UnityEngine.Object.DontDestroyOnLoad(_utilizationContainer);
        }

        public GameObject GetObject(GameObject prefab, Transform container = null)
        {
            // find object if exists
            foreach (var (itemPrefab, itemGameObject) in _pool)
            {
                if (!itemGameObject.activeInHierarchy
                    && itemGameObject.transform.parent == _utilizationContainer
                    && itemPrefab == prefab)
                {
                    itemGameObject.SetActive(true);
                    itemGameObject.transform.SetParent(container);
                    return itemGameObject;
                }
            }

            // create object if object not found
            var newGameObject = AddObject(prefab);
            newGameObject.transform.SetParent(container);
            newGameObject.SetActive(true);
            return newGameObject;
        }

        public T GetObject<T>(GameObject prefab, Transform container = null)
        {
            return GetObject(prefab, container).GetComponent<T>();
        }

        public GameObject AddObject(GameObject prefab)
        {
            var instance = UnityEngine.Object.Instantiate(prefab, _utilizationContainer);
            instance.SetActive(false);
            _pool.Add(Tuple.Create(prefab, instance));
            return instance;
        }

        public void UtilizeObject(GameObject utilizedGameObject)
        {
            utilizedGameObject.transform.SetParent(_utilizationContainer);
            utilizedGameObject.gameObject.SetActive(false);
        }

        public void UtilizeObject<T>(T utilizedGameObject) where T : MonoBehaviour
        {
            UtilizeObject(utilizedGameObject.gameObject);
        }

        private void OnDestroy()
        {
            _pool.Clear();
        }
    }
}