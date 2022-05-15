using UnityEngine;

namespace Asteroids
{
    public static class Bootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Connect()
        {
            var gameObjectPool = new GameObjectPool();

            var applicationTicker = new GameObject(nameof(ApplicationTicker))
                .AddComponent<ApplicationTicker>();
            Object.DontDestroyOnLoad(applicationTicker);

            new Game().Connect(gameObjectPool, applicationTicker);
        }
    }
}