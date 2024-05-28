using NJN.Runtime.Controllers;
using NJN.Runtime.Controllers.Enemy;
using System;
using UnityEngine;

namespace NJN.Runtime.Components
{
    public class EnemyAttack : BaseComponent, IAttack
    {
        [SerializeField] private float _attackRadius;
        [SerializeField] private LayerMask layerMask; 

        private EnemyController _enemyController;

        private float _hitTime = 0.2f;
        private float _recoveryTime = 0.3f;
        private float _timeCounter;
        private Action _updateFunction;
        private Action _endFunction;

        private void Start()
        {
            _enemyController = GetComponent<EnemyController>();
        }
        public void StartAttack(Action AttackEnd)
        {
            _endFunction = AttackEnd;

            _updateFunction = PreAttack;
            _timeCounter = 0;
        }
        public void CancellAttack()
        {
        }
        public void UpdateAttack()
        {
            _updateFunction();
        }
        private void PreAttack()
        {
            _timeCounter += Time.deltaTime;
            if (_timeCounter > _hitTime)
            {
                HitDamagable();
                _timeCounter = 0;
                _updateFunction = AfterAttack;
            }
        }

        private void AfterAttack()
        {
            _timeCounter += Time.deltaTime;
            if (_timeCounter > _recoveryTime)
            {
                if (_endFunction != null)
                {
                    _endFunction();
                }
                else
                    print("End Function should not be empty, critical error");
            }
        }

        private void HitDamagable()
        {
            Vector2 direction = Vector2.right;

            // Perform the CircleCast
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _attackRadius, layerMask);

            foreach (Collider2D collider in colliders)
            {
                IDamagable damagable = collider.GetComponent<IDamagable>();

                if (damagable != null)
                {
                    _enemyController.DamageProcessor.DealDamage(damagable);
                }
            }
        }
    }
}

