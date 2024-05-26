using System;
using NJN.Runtime.Controllers.Enemy;
using NJN.Runtime.Controllers.Player;
using NJN.Runtime.Factories;
using NJN.Runtime.Input;
using NJN.Runtime.Systems.Distraction;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace NJN.Runtime.Testing
{
    public class YaygunTest : MonoBehaviour
    {
        [SerializeField] private bool _fireDistraction;
        [SerializeField] private float _radius;
        [SerializeField] private float _time;

        private void Update()
        {
            if (_fireDistraction)
            {
                _fireDistraction = false;
                DistractionSystem.FireDistraction(transform.position, _radius, _time);
            }
        }
        [Button(ButtonSizes.Large)]
        private void ChasePlayer()
        {
            EnemyController enemy = GameObject.FindAnyObjectByType<EnemyController>();
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            enemy.ChaseState.StartChasing(player);
        }

        [Button(ButtonSizes.Large)]
        private void LostPlayer()
        {
            EnemyController enemy = GameObject.FindAnyObjectByType<EnemyController>();
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            enemy.ChaseState.LostLineOfSightToPlayer(player.transform.position);
        }

    }
}