using NJN.Runtime.Controllers;
using NJN.Runtime.Controllers.Enemy;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vit.Utilities;

namespace NJN.Runtime.Components
{
    public class Vision : BaseComponent
    {
        [SerializeField, ValueDropdown("GetLayerNames")]
        private string[] _selectedLayer;
        private float _visionDistance { get; } = 20;

        private float _visionFrequency = 0.2f;

        private IDamagable _damagable;

        private Transform _damagableTransform;

        private Vector3 _lastKnownPosition;

        private IEnumerable<string> GetLayerNames()
        {
            return Tools.GetLayerNames();
        }
        private EnemyController _enemyController;
        void Start()
        {
            _enemyController = GetComponent<EnemyController>();
            StartCoroutine(Look());
        }

        IEnumerator Look()
        {
            while (true)
            {
                yield return new WaitForSeconds(_visionFrequency);
                if (LookForDamagable())
                {
                    Detection();
                }                      
                else
                {
                    NoDetection();
                }
            }
        }
        private void Detection()
        {
            _enemyController.SetTargets(_damagableTransform);
            _enemyController.StateMachine.CurrentState.HasLineOfSightToPlayer();
        }
        private void NoDetection()
        {
            if (_damagable == null)
                return;

            _enemyController.StateMachine.CurrentState.NoLineOfSightToPlayer(_lastKnownPosition);
            _damagable = null;
            _damagableTransform = null;
        }
        private bool LookForDamagable()
        {
            Vector2 rayDirection = new Vector2(_enemyController.GetFaceDirectionAsValue(), 0);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, _visionDistance, LayerMask.GetMask(_selectedLayer));
            if (hit.collider == null)
            {
                return false;
            }

            IDamagable damagable = hit.collider.GetComponent<IDamagable>();
            if (damagable != null)
            {
                _damagable = damagable;
                _damagableTransform = hit.transform;
                _lastKnownPosition = _damagableTransform.position;
                return true;
            }
            else
            {
                return false;
            }
        }
        
    }
}
