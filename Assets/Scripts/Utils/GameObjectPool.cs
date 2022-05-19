using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class GameObjectPool
    {
        private readonly Transform _utilizationContainer;

        private readonly Dictionary<int, Stack<GameObject>> _gameObjectsById = new();
        private readonly Dictionary<GameObject, int> _prefabIdByGameObject = new();

        public GameObjectPool(Transform utilizationContainer = null)
        {
            _utilizationContainer = utilizationContainer != null
                ? utilizationContainer
                : new GameObject(nameof(GameObjectPool)).transform;
            UnityEngine.Object.DontDestroyOnLoad(_utilizationContainer);
        }

        public GameObject GetObject(GameObject prefab, Transform container = null)
        {
            var prefabId = prefab.GetInstanceID();

            if (_gameObjectsById.TryGetValue(prefab.GetInstanceID(), out var objects) &&
                objects.TryPop(out var gameObject))
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject = UnityEngine.Object.Instantiate(prefab, _utilizationContainer);
            }

            gameObject.transform.SetParent(container, false);
            _prefabIdByGameObject.Add(gameObject, prefabId);
            return gameObject;
        }

        public T GetObject<T>(GameObject prefab, Transform container = null) where T : MonoBehaviour
        {
            return GetObject(prefab, container).GetComponent<T>();
        }

        public void UtilizeObject(GameObject utilizedGameObject)
        {
            var prefabId = _prefabIdByGameObject[utilizedGameObject];

            utilizedGameObject.transform.SetParent(_utilizationContainer, false);
            utilizedGameObject.SetActive(false);

            if (!_gameObjectsById.TryGetValue(prefabId, out var gameObjects))
            {
                gameObjects = new Stack<GameObject>();
                _gameObjectsById.Add(prefabId, gameObjects);
            }

            gameObjects.Push(utilizedGameObject);
            _prefabIdByGameObject.Remove(utilizedGameObject);
        }

        public void UtilizeObject<T>(T utilizedGameObject) where T : MonoBehaviour
        {
            UtilizeObject(utilizedGameObject.gameObject);
        }

        private void OnDestroy()
        {
            _gameObjectsById.Clear();
            _prefabIdByGameObject.Clear();
        }
    }
}