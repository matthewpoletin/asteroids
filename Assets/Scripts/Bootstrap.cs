using UnityEngine;

namespace Asteroids
{
    public static class Bootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Connect()
        {
            var gameObjectPool = new GameObjectPool();

            new BattleModule().Connect(gameObjectPool);
        }
    }
}