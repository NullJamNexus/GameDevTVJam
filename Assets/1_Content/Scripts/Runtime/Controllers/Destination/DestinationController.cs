using System;
using System.Collections;
using System.Collections.Generic;
using NJN.Runtime.Controllers.Enemy;
using Unity.Mathematics;
using UnityEngine;

namespace NJN.Runtime.Controllers.Destination
{
    public class DestinationController : MonoBehaviour
    {
        private EnemyController[] _enemies;
        private Collider2D[] _colliders;
        private Dictionary<Collider2D, bool> _colliderInitialStates;

        private void Awake()
        {
            _enemies = GetComponentsInChildren<EnemyController>();
            _colliders = GetComponentsInChildren<Collider2D>();
            _colliderInitialStates = new Dictionary<Collider2D, bool>();

            foreach (Collider2D col in _colliders)
            {
                _colliderInitialStates[col] = col.enabled;
            }

            SetActiveStateForEntities(false);
        }

        public void MoveOldHouse(float accelerationRate, float maxSpeed)
        {
            StartCoroutine(LeaveTheScene(accelerationRate, maxSpeed));
        }

        public void MoveNewHouse(float startSpeed, float decelerationRate)
        {
            StartCoroutine(EnterTheScene(startSpeed, decelerationRate));
        }

        public void SetActiveStateForEntities(bool state)
        {
            foreach (EnemyController enemy in _enemies)
            {
                if (enemy != null)
                    enemy.gameObject.SetActive(state);
            }

            foreach (Collider2D col in _colliders)
            {
                if (col == null) continue;
                
                if (state)
                {
                    if (_colliderInitialStates.TryGetValue(col, out bool wasInitiallyActive) && wasInitiallyActive)
                    {
                        col.enabled = true;
                    }
                }
                else
                {
                    col.enabled = false;
                }
            }
        }

        private IEnumerator LeaveTheScene(float accelerationRate, float maxSpeed)
        {
            SetActiveStateForEntities(false);

            float speed = 0;
            while (true)
            {
                yield return null;
                speed += accelerationRate * Time.deltaTime;
                speed = math.min(speed, maxSpeed);
                transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            }
        }

        private IEnumerator EnterTheScene(float startSpeed, float decelerationRate)
        {
            float speed = startSpeed;
            while (speed > 0)
            {
                yield return null;
                transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
                speed -= decelerationRate * Time.deltaTime;
                speed = math.max(speed, 0);
            }

            SetActiveStateForEntities(true);
        }
    }
}
