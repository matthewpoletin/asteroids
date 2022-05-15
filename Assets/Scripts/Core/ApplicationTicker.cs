using System;
using UnityEngine;

namespace Asteroids.Core
{
    public class ApplicationTicker : MonoBehaviour
    {
        public event Action<float> OnTick;

        private void Update()
        {
            OnTick?.Invoke(Time.deltaTime);
        }
    }
}