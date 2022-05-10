using System;
using UnityEngine;

namespace Asteroids
{
    public class ApplicationTicker : MonoBehaviour
    {
        private Action<float> _onTick;

        public void Connect(Action<float> onTick)
        {
            _onTick = onTick;
        }

        private void Update()
        {
            _onTick?.Invoke(Time.deltaTime);
        }
    }
}