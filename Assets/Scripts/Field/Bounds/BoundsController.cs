using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids.Field
{
    public class BoundsController
    {
        private readonly float _minX;
        private readonly float _maxX;
        private readonly float _minY;
        private readonly float _maxY;

        private readonly List<IFieldActor> _fieldEntities = new();

        public BoundsController()
        {
            var mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("Main Camera was not found");
                return;
            }

            var orthographicHeight = mainCamera.orthographicSize;
            var orthographicWidth = orthographicHeight / mainCamera.pixelHeight * mainCamera.pixelWidth;

            var position = mainCamera.transform.position;
            _minX = position.x - orthographicWidth;
            _maxX = position.x + orthographicWidth;
            _minY = position.y - orthographicHeight;
            _maxY = position.y + orthographicHeight;
        }

        public Tuple<Vector2, Vector2, Vector2, Vector2> GetBoundsCornersPosition()
        {
            return new Tuple<Vector2, Vector2, Vector2, Vector2>(
                new Vector2(_minX, _minY),
                new Vector2(_minX, _maxY),
                new Vector2(_maxX, _maxY),
                new Vector2(_maxX, _minY)
            );
        }

        public void ValidateEntitiesPositions()
        {
            for (var index = _fieldEntities.Count - 1; index >= 0; index--)
            {
                var fieldEntity = _fieldEntities[index];
                var position = fieldEntity.Transform.position;

                var leftBounds = false;

                if (!(_minX <= position.x && position.x <= _maxX))
                {
                    position.x = -position.x;
                    leftBounds = true;
                }

                if (!(_minY <= position.y && position.y <= _maxY))
                {
                    position.y = -position.y;
                    leftBounds = true;
                }

                if (leftBounds)
                {
                    fieldEntity.Transform.position = position;
                    if (fieldEntity is IDestroyedFieldActor destroyedFieldActor)
                    {
                        destroyedFieldActor.OnLeavingBounds();
                    }
                }
            }
        }

        public void AddFieldEntity(IFieldActor fieldEntity)
        {
            _fieldEntities.Add(fieldEntity);
        }

        public void RemoveFieldEntity(IFieldActor fieldEntity)
        {
            _fieldEntities.Remove(fieldEntity);
        }

        public Vector2 GetSpawnPosition()
        {
            return new Vector2(Random.Range(_minX, _maxX), Random.Range(_minY, _maxY));
        }

        public void Reset()
        {
            _fieldEntities.Clear();
        }
    }
}