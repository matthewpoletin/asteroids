﻿using System;
using UnityEngine;

namespace Asteroids.Field
{
    public class LaserController : ITick
    {
        private readonly GameObjectPool _pool;
        private readonly Transform _followTransform;
        private readonly Action<LaserController> _onDeathCallback;

        private float _lifetimeTimer;

        private LaserView LaserView { get; }

        public LaserController(GameObjectPool pool, GameObject laserPrefab, Transform followTransform,
            Action<LaserController> onDeathCallback,
            float lifetimeDurationInSeconds)
        {
            _pool = pool;
            _followTransform = followTransform;
            _onDeathCallback = onDeathCallback;

            _lifetimeTimer = lifetimeDurationInSeconds;

            LaserView = _pool.GetObject<LaserView>(laserPrefab);
            LaserView.transform.SetPositionAndRotation(followTransform.position, followTransform.rotation);
            LaserView.Connect(lifetimeDurationInSeconds, OnTriggerEnter);
        }

        public void Tick(float deltaTime)
        {
            if (_lifetimeTimer > 0)
            {
                _lifetimeTimer -= deltaTime;
                if (_lifetimeTimer <= 0)
                {
                    _onDeathCallback?.Invoke(this);
                }
            }

            LaserView.transform.SetPositionAndRotation(_followTransform.position, _followTransform.rotation);
        }

        public void Utilize()
        {
            _pool.UtilizeObject(LaserView);
        }

        private void OnTriggerEnter(Collider2D other)
        {
            if (other.TryGetComponent<IReceivingDamage>(out var target))
            {
                target.Kill();
            }
        }
    }
}