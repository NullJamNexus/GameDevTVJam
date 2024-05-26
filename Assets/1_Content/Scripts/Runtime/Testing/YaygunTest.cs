using System;
using System.Collections.Generic;
using NJN.Runtime.Controllers.Enemy;
using NJN.Runtime.Systems.Distraction;
using Sirenix.OdinInspector;
using UnityEngine;
using Vit.Utilities;


namespace NJN.Runtime.Testing
{
    public class YaygunTest : MonoBehaviour
    {
        [SerializeField, ValueDropdown("GetLayerNames")]
        private string _selectedLayer;

        private IEnumerable<string> GetLayerNames()
        {
            return Tools.GetLayerNames();
        }

        [SerializeField] private bool _fireDistraction;
        [SerializeField] private float _radius;
        [SerializeField] private float _time;

        [Button(ButtonSizes.Large)]
        private void FireDistraction()
        {
            DistractionSystem.FireDistraction(transform.position, _radius, _time, LayerMask.GetMask(_selectedLayer));          
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